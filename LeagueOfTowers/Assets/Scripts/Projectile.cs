using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //the target monster
    private Monster target;

    //tower that the projectile comes from
    private Tower parent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //move towards the target
        MoveToTarget();
    }

    //initialize the projectile's tower parent and target
    public void Initialize(Tower towerParent)
    {
        this.target = towerParent.getTarget();
        this.parent = towerParent;
    }

    private void MoveToTarget()
    {
        if (target != null && target.IsActive)
        {
            //move from the origin into the target monster based on projectilespeed + ms of a frame
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * parent.getProjectileSpeed());

            //transform the direction of the projectile
            Vector2 projectileDir = target.transform.position - transform.position;

            //get the angle needed to transform the projectile direction
            float angle = Mathf.Atan2(projectileDir.y, projectileDir.x) * Mathf.Rad2Deg;

            //rotate the projectile in the proper direction
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        } else if (!target.IsActive) //if the target is gone/dead
        {
            //release the game object
            GameManager.Instance.Pool.ReleaseObject(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Monster") //if the target in range is a monster
        {
            if (target.gameObject == other.gameObject)
            {
                //Debug.Log("Monster hit");

                target.TakeDamage(parent.getDamage());

                //remove the projectile from the pool of objects in scene
                GameManager.Instance.Pool.ReleaseObject(gameObject);
            }

        }
    }
}
