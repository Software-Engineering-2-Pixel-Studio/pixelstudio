using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class Tower : MonoBehaviour
{
    //price of the tower
    //private string towerName;
    [SerializeField]
    private int price;  //done
    private PhotonView view;    //done

    //tower range renderer
    private SpriteRenderer mySpriteRenderer;    //done

    //target monster
    private Monster target; //done

    //pool of monsters that enter the tower
    private Queue<Monster> monsters = new Queue<Monster>(); //done

    //projectile type
    [SerializeField] private string projectileType; //done

    //projectile speed
    [SerializeField] private float projectileSpeed; //removed

    //damage of the tower
    [SerializeField] private int damage;    //done

    //let's say we can attack from the getgo
    private bool canAttack = true;  //done

    //how fast/often we can attack
    private float attackTimer;  //done

    //how long till the next attack
    [SerializeField] private float attackCooldown;  //done

    private Point placedAtTile;     //tile grid position of this tower  //removed

    // Start is called before the first frame update
    private void Start()    //done
    {
        this.view = this.GetComponentInParent<PhotonView>();
        //get the sprite
        this.mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    private void Update()   //done
    {
        //attack targets
        //Attack();

        //Debug.Log(target);
    }
    // public void OnPhotonInstantiate(PhotonMessageInfo info)
    // {
    //     var parent = GameObject.Find("Root");

    //     this.transform.SetParent(parent.transform, true);
    // }

    public void setPrice(int otherPrice)    //removed
    {
        this.price = otherPrice;
    }

    public void setDamage(int damageGiven)     //removed
    {
        this.damage = damageGiven;

    }
    public void Select()    //done
    {
        //enable or disable the tower range when selected
        mySpriteRenderer.enabled = !mySpriteRenderer.enabled;
    }

    public void Attack()    //done
    {
        //if we can't attack
        if (!canAttack)
        {
            //add millisecond count to attack timer
            attackTimer += Time.deltaTime;

            //if attack timer is more than the cooldown now
            if (attackTimer >= attackCooldown)
            {
                //we can attack again and reset the timer
                canAttack = true;
                attackTimer = 0;
            }
        }

        //if the target doesn't exist anymore in the tower range and there are still monsters in the queue
        if (target == null && monsters.Count > 0)
        {
            //remove the monster from the queue
            target = monsters.Dequeue();
        }

        //if we have a target and target is active
        if (target != null && target.IsActive)
        {
            //if we can attack
            if(canAttack)
            {
                //shoot then stop attacking
                Shoot();
                canAttack = false;
            }
        }
    }

    /*
        Method to shoot projectile
    */
    public void Shoot() //nearly done
    {
        //get the projectile type from the pool and return it
        GameObject projectile = PhotonNetwork.Instantiate(this.projectileType, this.transform.position, Quaternion.identity,0,null);


        //initialize the projectile by passing this tower
        projectile.GetComponent<Projectile>().Initialize(this);
    }

    //this function happens when it enters the tower range
    public void OnTriggerEnter2D(Collider2D other)  //done
    {
        //if the target is a monster
        if (other.tag == "Monster")
        {
            //put that monster inside the queue
            monsters.Enqueue(other.GetComponent<Monster>());
        }
    }

    //this function happens when it exits the tower range
    public void OnTriggerExit2D(Collider2D other)   //done
    {
        //if the target is a monster
        if (other.tag == "Monster")
        {
            //remove the target
            target = null;
        }
    }
    
    /*
        Method to get projectile speed
    */
    public float getProjectileSpeed()   //removed
    {
        return projectileSpeed;
    }

    /*
        Method to get targer monster of this tower
    */
    public Monster getTarget()
    {
        return target;
    }

    /*
        Method to get damage of this tower
    */
    public int getDamage(){
        return this.damage;
    }

    /*
        Method to get price of this tower
    */
    public int getPrice(){
        return this.price;
    }

    /*
        Method to get PhotonView of this tower
    */
    public PhotonView GetPhotonView(){
        return this.view;
    }

    /*
        Destroy this tower
    */
    public void DestroyThisTower(){
        //only the owner can destroy this tower
        if(this.view.IsMine){
            //Tower Prefabs has child Range which stores Tower.cs(this) Script.
            PhotonNetwork.Destroy(this.transform.parent.gameObject);
        }
    }

    /*
        Set the tile grid position for this tower
    */

    public void SetPlacedAtTile(Point tileGridPosition){
        this.placedAtTile = tileGridPosition;
    }

    /*
        Get the tile grid position of this tower
    */
    public Point GetPlacedAtTile(){
        return this.placedAtTile;
    }

}
