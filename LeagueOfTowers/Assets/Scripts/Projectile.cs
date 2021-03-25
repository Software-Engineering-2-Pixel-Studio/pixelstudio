using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //fields
    private Monster target; //the target monster

    private Tower parent;   //tower that the projectile comes from

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
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

    /*
        Method for moving the projectile to target monster
    */
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
}
