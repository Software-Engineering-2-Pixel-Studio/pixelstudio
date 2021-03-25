using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
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
    

    //let's say we can attack from the getgo
    private bool canAttack = true;

    //how fast/often we can attack
    private float attackTimer;

    //how long till the next attack
    [SerializeField] private float attackCooldown;

    //price of the tower
    private int price;

    //damage of the tower
    [SerializeField] private int damage;

    // Start is called before the first frame update
    void Start()
    {
        //get the sprite
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
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
        else if(monsters.Count > 0)
        {
            target = monsters.Dequeue();
        }

        if (target != null)
        {
            if(!target.isAlive || !target.IsActive) //if we have a target and it's not alive nor active
            {
                target = null;
            }
        }
    }

    public void Shoot()
    {
        //get the projectile type from the pool and return it
        Projectile projectile = GameManager.Instance.Pool.GetObject(projectileType).GetComponent<Projectile>();

        //spawn the projectile from the middle of the tower
        projectile.transform.position = transform.position;

        //initialize the projectile by passing this tower
        projectile.Initialize(this);
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
            GameObject gameObj = other.gameObject;

            if(gameObj.activeInHierarchy)
            {
                //remove the target
                target = null;
            }

            
        }
    }
    public float getProjectileSpeed()
    {
        return projectileSpeed;
    }

    public Monster getTarget()
    {
        return target;
    }

    public int getPrice()
    {
        return price;
    }

    public int getDamage()
    {
        return damage;
    }
}
