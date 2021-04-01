using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public enum Element
{
    FIRE,
    LIGHT,
    SLOW,
    NONE
}


public abstract class Tower : MonoBehaviour
{
    //price of the tower
    [SerializeField]
    private int price;
    private PhotonView view;

    //tower range renderer
    private SpriteRenderer mySpriteRenderer;

    //target monster
    private Monster target;

    //pool of monsters that enter the tower
    private Queue<Monster> monsters = new Queue<Monster>();

    //projectile type
    [SerializeField] private string projectileType;

    //projectile speed
    [SerializeField] private float projectileSpeed;

    //damage of the tower
    [SerializeField] private int damage;

    //debuff duration of the tower
    [SerializeField] private float debuffDuration;

    //chance for debuff to be applied
    [SerializeField] private float debuffProcChance;

    //let's say we can attack from the getgo
    private bool canAttack = true;

    //how fast/often we can attack
    private float attackTimer;

    //how long till the next attack
    [SerializeField] private float attackCooldown;

    private Point placedAtTile;     //tile grid position of this tower

    private Element elementType;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Set();
    }

    public void Set()
    {
        //get the sprite
        this.mySpriteRenderer = GetComponent<SpriteRenderer>();
        this.view = this.GetComponentInParent<PhotonView>();
    }

    // Update is called once per frame
    private void Update()
    {
        //attack targets
        Attack();

        //Debug.Log(target);
    }

    public void setPrice(int otherPrice)
    {
        this.price = otherPrice;
    }

    public void setDamage(int damageGiven)
    {
        this.damage = damageGiven;

    }

    protected void setElementType(Element otherElement)
    {
        this.elementType = otherElement;
    }

    public void setDebuffDuration(float givenDebuffDuration)
    {
        this.debuffDuration = givenDebuffDuration;
    }

    public void setDebuffProcChance(float givenDebuffProc)
    {
        this.debuffProcChance = givenDebuffProc;
    }

    public void Select()
    {
        //enable or disable the tower range when selected
        mySpriteRenderer.enabled = !mySpriteRenderer.enabled;
    }

    public void Attack()
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
        else if (monsters.Count > 0) //if there are still monsters
        {
            //change target
            target = monsters.Dequeue();
        }

        //reset the target when monster enters the portal
        if (target != null && !target.isAlive)
        {
            target = null;
        }
    }

    /*
        Method to shoot projectile
    */
    public void Shoot()
    {
        //get the projectile type from the pool and return it
        GameObject projectile = PhotonNetwork.Instantiate(this.projectileType, this.transform.position, Quaternion.identity,0,null);


        //initialize the projectile by passing this tower
        projectile.GetComponent<Projectile>().Initialize(this);
    }

    //this function happens when it enters the tower range
    public void OnTriggerEnter2D(Collider2D other)
    {
        //if the target is a monster
        if (other.tag == "Monster")
        {
            //put that monster inside the queue
            monsters.Enqueue(other.GetComponent<Monster>());
        }
    }

    //this function happens when it exits the tower range
    public void OnTriggerExit2D(Collider2D other)
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
    public float getProjectileSpeed()
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
        Method to get element of this tower
    */
    public Element getElementType()
    {
        return this.elementType;
    }

    /*
        Method to get debuff duration of this tower
    */
    public float getDebuffDuration()
    {
        return this.debuffDuration;
    }

    /*
        Method to get debuff proc chance of this tower
    */
    public float getDebuffProcChance()
    {
        return this.debuffProcChance;
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

    /*
        Get the debuff of this tower
    */
    public abstract Debuff GetDebuff();

    
}
