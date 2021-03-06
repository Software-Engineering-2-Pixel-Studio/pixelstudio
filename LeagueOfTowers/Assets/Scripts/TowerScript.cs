using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class TowerScript : MonoBehaviourPun, IPunInstantiateMagicCallback
{
    //field
    //these need initialize at Instantiate
    protected string towerName;       //0 //tower's name
    [SerializeField] protected int price;              //1 //tower's price
    [SerializeField] protected float damage;           //2 //tower's damage
    protected string projectileType;  //3 //tower's projectile type
    [SerializeField] protected float projectileSpeed;  //4 //tower's projectile speed
    [SerializeField] protected float attackCooldown;   //5 //tower's attack cool down
    protected int parentTileID;       //7 //tileID where tower is placed
    protected int towerID;            //6 //towerID
    [SerializeField] protected Element myElement; //8
    [SerializeField] protected float debuffChance;    //9
    [SerializeField] protected float duration;   //10
    [SerializeField] protected int level;
    protected int nextUpgradeLevel;

    [SerializeField] protected bool upgradedTech = false;

    //these will initialized in run-time       
    protected bool canAttack = true;      //state of attack
    protected float attackTimer;          //time counting for next attack
    protected MonsterScript target;       //tower's target
    protected Queue<MonsterScript> monsters = new Queue<MonsterScript>();
    protected TowerUpgrade[] upgrades = new TowerUpgrade[2];
    protected TowerUpgrade nextUpgrade;

    protected TechUpgrade techUpgrade = new TechUpgrade(0.0f, 0.0f);
    
    [SerializeField] protected GameObject projectilePrefab;   //projectile prefab 

    //components
    protected RangeScript myRangeScript;
    protected PhotonView view;

    
    // Start is called before the first frame update
    protected virtual void Start()
    {
        this.view = this.GetComponent<PhotonView>();
        this.myRangeScript = this.GetComponentInChildren<RangeScript>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(this.view.IsMine){
            Attack();
        }
    }

    //get/set fields
    public int GetPrice()
    {
        return this.price;
    }

    public float GetDamage()
    {
        return this.damage;
    }

    public string GetProjectileType()
    {
        return this.projectileType;
    }

    public float GetProjectileSpeed()
    {
        return this.projectileSpeed;
    }

    public int GetParentTileID()
    {
        return this.parentTileID;
    }

    public MonsterScript GetTargetMonster()
    {
        return this.target;
    }

    public GameObject GetProjectilePrefab()
    {
        return this.projectilePrefab;
    }

    public int GetTowerViewID()
    {
        return this.view.ViewID;
    }

    /*
        Method to get debuff duration of this tower
    */
    public float getDebuffDuration()
    {
        return this.duration;
    }

    /*
        Method to get debuff proc chance of this tower
    */
    public float getDebuffProcChance()
    {
        return this.debuffChance;
    }

    public int GetLevel()
    {
        return this.level;
    }

    public TowerUpgrade GetNextUpgrade()
    {
        return this.nextUpgrade;
    }

    public abstract Debuff GetDebuff();

    public virtual string GetStats()
    {
        //set the default string passed to tower stats panel
        string result = string.Format("\nLevel: {0} \nDamage: {1} \nProc: {2}% \nDebuff: {3} sec \nElement: {4}", this.level, this.damage, this.debuffChance, this.duration, this.myElement.ToString());

        if(this.nextUpgrade != null)
        {
            result = string.Format("\nLevel: {0} \nDamage: {1} <color=#00ff00ff>+{4}</color>\nProc; {2}% <color=#00ff00ff>+{5}%</color>\nDebuff: {3} sec <color=#00ff00ff>+{6}</color> \nElement: {7}",
                this.level, this.damage, this.debuffChance, this.duration, this.nextUpgrade.Damage, this.nextUpgrade.DebuffProcChance, this.nextUpgrade.DebuffDuration, this.myElement);
        }

        return result;
    }

    public virtual string GetTechStats()
    {
        string result = string.Format("\nProjectile Speed: {0} \nAttack Cooldown: {1}", this.projectileSpeed, this.attackCooldown);
        
        //if the tower's tech is not upgraded yet
        if(!this.upgradedTech)
        {
            result = string.Format("\nProjectile Speed: {0} <color=#00ff00ff>+{2}</color> \nAttack Cooldown: {1} <color=#00ff00ff> {3}</color>", this.projectileSpeed, this.attackCooldown, this.techUpgrade.ProjectileSpeed, this.techUpgrade.AttackCoolDown);
        }

        return result;
    }

    public bool UpgradedTech()
    {
        return this.upgradedTech;
    }

    protected void setNextUpgrade()
    {
        //check if it has the proper upgrade first then return the proper upgrade
        if(this.upgrades.Length == 2)
        {
            if(this.nextUpgradeLevel == 2)
            {
                //return upgrade level 2
                this.nextUpgrade = upgrades[0];
            }
            else if(this.nextUpgradeLevel == 3)
            {
                //return upgrade level 3
                this.nextUpgrade = upgrades[1];
            }
            else{
                nextUpgrade = null;
            }
        }
        else
        {
            nextUpgrade = null;
        }
        
    }

    //sets-up all the upgrade values
    private void setUpgrades()
    {
        //sets up the values for upgrades 1, 2 and tech upgrade
        List<object> upgrade1 = new List<object>();
        List<object> upgrade2 = new List<object>();
        List<object> techUp = new List<object>();

        if(this.towerName == "BaseTower")
        {
            upgrade1 = TowerData.GetBaseTowerUp1Data();
            
            if(upgrade1.Count == 2)
            {
                this.upgrades[0] = new TowerUpgrade((int) upgrade1[0], (float) upgrade1[1]);
            }
            upgrade2 = TowerData.GetBaseTowerUp2Data();
            if(upgrade2.Count == 2)
            {
                this.upgrades[1] = new TowerUpgrade((int) upgrade2[0], (float) upgrade2[1]);
            }

            techUp = TowerData.GetBaseTowerTechUpData();
            this.techUpgrade = new TechUpgrade((float) techUp[0], (float) techUp[1]);
        }
        else if(this.towerName == "FireTower")
        {
            upgrade1 = TowerData.GetFireTowerUp1Data();
            if(upgrade1.Count == 6)
            {
                this.upgrades[0] = new TowerUpgrade((int) upgrade1[0], (float) upgrade1[1],
                                                    (float) upgrade1[2], (float) upgrade1[3],
                                                    (float) upgrade1[4], (float) upgrade1[5]);
            }
            upgrade2 = TowerData.GetFireTowerUp2Data();
            if(upgrade2.Count == 6)
            {
                this.upgrades[1] = new TowerUpgrade((int) upgrade2[0], (float) upgrade2[1],
                                                    (float) upgrade2[2], (float) upgrade2[3],
                                                    (float) upgrade2[4], (float) upgrade2[5]);
            }

            techUp = TowerData.GetFireTowerTechUpData();
            this.techUpgrade = new TechUpgrade((float) techUp[0], (float) techUp[1]);
        }
        else if(this.towerName == "SlowTower")
        {
            upgrade1 = TowerData.GetSlowTowerUp1Data();
            if(upgrade1.Count == 5)
            {
                this.upgrades[0] = new TowerUpgrade((int) upgrade1[0], (float) upgrade1[1],
                                                    (float) upgrade1[2], (float) upgrade1[3],
                                                    (float) upgrade1[4]);
            }
            upgrade2 = TowerData.GetSlowTowerUp2Data();
            if(upgrade2.Count == 5)
            {
                this.upgrades[1] = new TowerUpgrade((int) upgrade2[0], (float) upgrade2[1],
                                                    (float) upgrade2[2], (float) upgrade2[3],
                                                    (float) upgrade2[4]);
            }

            techUp = TowerData.GetSlowTowerTechUpData();
            this.techUpgrade = new TechUpgrade((float) techUp[0], (float) techUp[1]);
        }
        else if(this.towerName == "LightTower"){
            upgrade1 = TowerData.GetLightTowerUp1Data();
            if(upgrade1.Count == 4)
            {
                this.upgrades[0] = new TowerUpgrade((int) upgrade1[0], (float) upgrade1[1],
                                                    (float) upgrade1[2], (float) upgrade1[3]);
            }
            upgrade2 = TowerData.GetLightTowerUp2Data();
            if(upgrade2.Count == 4)
            {
                this.upgrades[1] = new TowerUpgrade((int) upgrade2[0], (float) upgrade2[1],
                                                    (float) upgrade2[2], (float) upgrade2[3]);
            }

            techUp = TowerData.GetLightTowerTechUpData();
            this.techUpgrade = new TechUpgrade((float) techUp[0], (float) techUp[1]);
        }
    }

    //events
    /*
        initialize towerscript when its tower object is instantiated over network
    */
    public virtual void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        //Debug.Log("TowerScript base is called!");
        object[] data = this.gameObject.GetPhotonView().InstantiationData;
        if(data != null && data.Length >= 11){
            //name
            this.towerName = (string) data[0];
            //price
            this.price = (int) data[1];
            //damage
            this.damage = (float) data[2];
            //projectileType
            this.projectileType = (string) data[3];
            //projectile speed
            this.projectileSpeed = (float) data[4];
            //cooldown
            this.attackCooldown = (float) data[5];
            //tileID
            this.parentTileID = (int) data[7];
            //base id
            this.towerID = (int) data[6] + this.parentTileID;
            this.gameObject.name = this.towerName + this.towerID;
            //elemetn
            this.myElement = (Element) ( (byte) data[8]);
            //debuffchance
            this.debuffChance = (float) data[9];
            //duration
            this.duration = (float) data[10];
            //level
            this.level = 1;
            //nextUpgradeLevel
            this.nextUpgradeLevel = 2;
            //upgradedTech state
            this.upgradedTech = false;
            //Debug.Log("Element: " + myElement.ToString());
            GameObject tileGO = MapManager.Instance.getTile(this.parentTileID);
            Tile tile = tileGO.GetComponent<Tile>();
            
            //set its parent object
            this.transform.parent = tileGO.transform;

            //set upgrades
            this.setUpgrades();
            this.setNextUpgrade();
        }
    }

    /*
        this method called when an enemy go in the range of tower
    */
    public void OnTriggerEnter2DRange(Collider2D other)
    {
        //if the target is a monster
        if (other.tag == "Monster")
        {
            //put that monster inside the queue
            monsters.Enqueue(other.GetComponent<MonsterScript>());
        }
    }

    /*
        this method called when an enemy go out of the range of tower
    */
    public void OnTriggerExit2DRange(Collider2D other)
    {
        //if the target is a monster
        if (other.tag == "Monster")
        {
            //remove the target
            target = null;
        }
    }

    //actions
    /*
        toggle the visual range of tower
    */
    public void Select()
    {
        if(this.myRangeScript != null){
            this.myRangeScript.ToggleRangeSR();
        }
    }

    /*
        attack function of tower with a timer base on the cool down
    */
    public void Attack()
    {
        //if we can't attack
        if (!this.canAttack)
        {
            //add millisecond count to attack timer
            this.attackTimer += Time.deltaTime;

            //if attack timer is more than the cooldown now
            if (this.attackTimer >= this.attackCooldown)
            {
                //we can attack again and reset the timer
                this.canAttack = true;
                this.attackTimer = 0;
            }
        }

        //if the target doesn't exist anymore in the tower range and there are still monsters in the queue
        if (this.target == null && this.monsters.Count > 0 && monsters.Peek().GetIsActive())
        {
            //remove the monster from the queue
            this.target = this.monsters.Dequeue();
        }

        //if we have a target and target is active
        if (this.target != null && this.target.GetIsActive())
        {
            //if we can attack
            if(this.canAttack)
            {
                //shoot then stop attacking
                shoot();
                this.canAttack = false;
            }
        }

        //reset the target when monster enters the portal
        if (target != null && !target.GetIsAlive() || target != null && !target.GetIsActive())
        {
            target = null;
        }
    }

    /*
        create projectile over the network
    */
    private void shoot()
    {
        object[] data = new object[5];
        data[0] = (string) this.projectileType;
        data[1] = (float) this.projectileSpeed;
        data[2] = (float) this.damage;
        data[3] = (int) this.view.ViewID;  //viewID of this tower
        data[4] = (int) this.target.GetMonsterViewID(); //viewID of target monster
        
        GameObject projectile = PhotonNetwork.Instantiate(projectilePrefab.name, this.transform.position, Quaternion.identity,0, data );        
    }

    //send upgrade signal to all
    public virtual void Upgrade()
    {
        if(this.view.IsMine)
        {
            this.view.RPC("upgradeRPC", RpcTarget.All);
        }
    }

    public virtual void TechUpgrade()
    {
        //only owner using this
        if(this.view.IsMine)
        {   
            //only this owner's token is reduce
            LevelUpManager.Instance.SpendToken();

            //send signal to other player about this tower's stat change
            this.view.RPC("techUpgradeRPC", RpcTarget.All);
        }
    }

    /*
        method to destroy this tower over network
    */
    public void DestroyThisTower(){
        //only the owner can destroy this tower
        if(this.view.IsMine){
            //Tower Prefabs has child Range which stores Tower.cs(this) Script.
            PhotonNetwork.Destroy(this.transform.gameObject);
        }
    }

    /*
        ToString method for simply extract this object's information
    */
    public override string ToString()
    {
        return this.towerName;
    }

    //punRPC
    [PunRPC]
    private void upgradeRPC()
    {
        //decrease shared global currency
        if(PhotonNetwork.IsMasterClient){
            CurrencyManager.Instance.SubCurrency(this.nextUpgrade.Price);
        }
        
        //increase price of this tower
        this.price += this.nextUpgrade.Price;

        //increase the damage, debuff proc chance, and debuff duration of tower
        this.damage += this.nextUpgrade.Damage;
        this.debuffChance += this.nextUpgrade.DebuffProcChance;
        this.duration += this.nextUpgrade.DebuffDuration;

        //increase tower level
        this.level++;
        this.nextUpgradeLevel++;
        setNextUpgrade();
    }

    //punRPC
    [PunRPC]
    private void techUpgradeRPC()
    {
        //this tower is tech upgraded
        this.upgradedTech = true;
        
        //increase the projectile speed and improve attack cooldown
        this.projectileSpeed += this.techUpgrade.ProjectileSpeed;
        this.attackCooldown += this.techUpgrade.AttackCoolDown;
    }
}
