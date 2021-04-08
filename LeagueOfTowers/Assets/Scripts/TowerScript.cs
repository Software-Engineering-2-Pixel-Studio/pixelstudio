using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class TowerScript : MonoBehaviourPun, IPunInstantiateMagicCallback
{
    //field
    // private string towerName;       //0 //tower's name
    // private int price;              //1 //tower's price
    // private float damage;           //2 //tower's damage
    // private string projectileType;  //3 //tower's projectile type
    // private float projectileSpeed;  //4 //tower's projectile speed
    // private float attackCooldown;   //5 //tower's attack cool down
    // private int parentTileID;       //7 //tileID where tower is placed
    // private int towerID;            //6 //towerID
    // private bool canAttack = true;      //state of attack
    // private float attackTimer;          //time counting for next attack
    // private MonsterScript target;       //tower's target
    // private Queue<MonsterScript> monsters = new Queue<MonsterScript>();
    
    // [SerializeField] private GameObject projectilePrefab;   //projectile prefab

    // //components
    // private RangeScript myRangeScript;
    // private PhotonView view;

    protected string towerName;       //0 //tower's name
    protected int price;              //1 //tower's price
    protected float damage;           //2 //tower's damage
    protected string projectileType;  //3 //tower's projectile type
    protected float projectileSpeed;  //4 //tower's projectile speed
    protected float attackCooldown;   //5 //tower's attack cool down
    protected int parentTileID;       //7 //tileID where tower is placed
    protected int towerID;            //6 //towerID
    [SerializeField] protected Element myElement; //8
    protected bool canAttack = true;      //state of attack
    protected float attackTimer;          //time counting for next attack
    protected MonsterScript target;       //tower's target
    protected Queue<MonsterScript> monsters = new Queue<MonsterScript>();
    
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

    //events
    /*
        initialize towerscript when its tower object is instantiated over network
    */
    public virtual void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        //Debug.Log("TowerScript base is called!");
        object[] data = this.gameObject.GetPhotonView().InstantiationData;
        if(data != null && data.Length >= 9){
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
            Debug.Log("Element: " + myElement.ToString());
            GameObject tileGO = MapManager.Instance.getTile(this.parentTileID);
            Tile tile = tileGO.GetComponent<Tile>();

            //set its parent object
            this.transform.parent = tileGO.transform;
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
        if (this.target == null && this.monsters.Count > 0)
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
}
