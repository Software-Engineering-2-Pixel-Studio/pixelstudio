using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TowerScript : MonoBehaviourPun, IPunInstantiateMagicCallback
{
    //field
    private string towerName;       //0
    private int price;              //1
    private float damage;           //2
    private string projectileType;  //3
    private float projectileSpeed;  //4
    private float attackCooldown;   //5
    private int towerID;
    private bool canAttack = true;
    private float attackTimer; 
    private MonsterScript target;
    private Queue<MonsterScript> monsters = new Queue<MonsterScript>();
    private int parentTileID;       //6
    //private Point placedAtGridPostion; //maybe change to tileID-> has to change structure of Dictionary of tiles in MapManager 
    
    [SerializeField] private GameObject projectilePrefab;

    //components
    private RangeScript myRangeScript;
    private PhotonView view;

    
    // Start is called before the first frame update
    void Start()
    {
        //this.rangeTrans = this.transform.GetChild(0).transform;
        this.view = this.GetComponent<PhotonView>();
        this.myRangeScript = this.GetComponentInChildren<RangeScript>();
    }

    // Update is called once per frame
    void Update()
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
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        Debug.Log("called!");
        object[] data = this.gameObject.GetPhotonView().InstantiationData;
        if(data != null && data.Length == 8){
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
            
            GameObject tileGO = MapManager.Instance.getTile(this.parentTileID);
            Tile tile = tileGO.GetComponent<Tile>();

            //set its parent object
            this.transform.parent = tileGO.transform;
            //this.placedAtGridPostion = tile.GetTilePosition();

            //MapManager.Instance.SetTileTower(this.parentTileID, this);

        }
    }

    public void OnTriggerEnter2DRange(Collider2D other)
    {
        //if the target is a monster
        if (other.tag == "Monster")
        {
            //put that monster inside the queue
            monsters.Enqueue(other.GetComponent<MonsterScript>());
        }
    }

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
    public void Select()
    {
        //enable or disable the tower range when selected
        //mySpriteRenderer.enabled = !mySpriteRenderer.enabled;
        if(this.myRangeScript != null){
            this.myRangeScript.ToggleRangeSR();
        }
        
    }

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

    // [PunRPC]
    // private void shootRPC(int towerViewID, int monsterViewID, float speed)
    // {
    //     //get the projectile type from the pool and return it
    //     //GameObject projectile = PhotonNetwork.Instantiate(this.projectileType, this.transform.position, Quaternion.identity,0,null);
    //     Debug.Log("shootRPC is called");

    //     //initialize the projectile by passing this tower
    //     //projectile.GetComponent<Projectile>().Initialize(this);
    //     GameObject projectile;

        
        
    //     Transform towerTrans = PhotonView.Find(towerViewID).transform;
    //     //TowerScript towerScript = towerPV.gameObject.GetComponent<TowerScript>();

    //     Transform monsterTrans = PhotonView.Find(monsterViewID).transform;
    //     //Monster monsterScript = monsterPV.gameObject.GetComponent<Monster>();

    //     GameObject prefab = this.projectilePrefab;
    //     //projectile = (GameObject) Instantiate(this.projectilePrefab, this.transform.position, Quaternion.identity);
    //     projectile = (GameObject) Instantiate(prefab, towerTrans.position, Quaternion.identity);
    //     projectile.GetComponent<ProjectileScript>().InitializeProjectile(towerTrans.GetComponent<TowerScript>(), monsterTrans.GetComponent<Monster>() , speed);
        
        
    // }

    // public void Shoot()
    // {
    //     this.view.RPC("shootRPC", RpcTarget.All, this.view.ViewID, this.target.GetComponent<PhotonView>().ViewID, this.projectileSpeed);
        
    // }
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

    public void DestroyThisTower(){
        //only the owner can destroy this tower
        if(this.view.IsMine){
            //Tower Prefabs has child Range which stores Tower.cs(this) Script.
            PhotonNetwork.Destroy(this.transform.gameObject);
        }
    }

    public override string ToString()
    {
        return this.towerName;
    }
}
