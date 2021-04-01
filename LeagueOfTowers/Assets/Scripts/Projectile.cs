using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Projectile : MonoBehaviour
{
    //fields
    private Monster targetMonster; //the target monster

    private Tower parentTower;   //tower that the projectile comes from

    private Element elementType;

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        //move towards the target
        MoveToTarget();
        //if target is destroyed before this projectile hit, destroy this projectile
        if(PhotonNetwork.IsMasterClient){
            if(targetMonster == null){
                PhotonNetwork.Destroy(this.transform.gameObject);
            }
        }
    }

    //initialize the projectile's tower parent and target as well as element type
    public void Initialize(Tower towerParent)
    {
        this.transform.SetParent(towerParent.transform);
        this.parentTower = this.GetComponentInParent<Tower>();
        this.targetMonster = parentTower.getTarget();
        this.elementType = parentTower.getElementType();
    }

    /*
        Method for moving the projectile to target monster
    */
    private void MoveToTarget()
    {
        if(targetMonster == null){
            return;
        }
        
        else if (targetMonster != null && targetMonster.IsActive)
        {
            //move from the origin into the target monster based on projectilespeed + ms of a frame
            transform.position = Vector3.MoveTowards(transform.position, targetMonster.transform.position, Time.deltaTime * parentTower.getProjectileSpeed());

            //transform the direction of the projectile
            Vector2 projectileDir = targetMonster.transform.position - transform.position;

            //get the angle needed to transform the projectile direction
            float angle = Mathf.Atan2(projectileDir.y, projectileDir.x) * Mathf.Rad2Deg;

            //rotate the projectile in the proper direction
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        } 
        else if (!targetMonster.IsActive) //if the target is gone/dead
        {
            //release the game object
            GameManager.Instance.Pool.ReleaseObject(gameObject);
        }
        
    }

    private void ApplyDebuff()
    {
        //if target monster type is different than projectile element type
        if (targetMonster.getElementType() != this.elementType)
        {
            //roll a chance number for debuff
            float rollNum = Random.Range(0, 100);

            //add the debuff if roll is more than or equal to proc chance
            if (rollNum <= parentTower.getDebuffProcChance())
            {
                //get parent tower's debuff and apply it to target monster
                targetMonster.AddDebuff(parentTower.GetDebuff());
            }
        }

        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Monster") //if the target in range is a monster
        {
            if(targetMonster == null){
                PhotonNetwork.Destroy(this.transform.gameObject);
                return;
            }
            else if (targetMonster.gameObject == other.gameObject)
            {
                //Debug.Log("Monster hit");

                targetMonster.TakeDamage(parentTower.getDamage(), elementType);

                ApplyDebuff();

                //remove the projectile from the pool of objects in scene
                GameManager.Instance.Pool.ReleaseObject(gameObject);
                
                //destroy it
                PhotonNetwork.Destroy(this.transform.gameObject);
            }

        }
    } 
    
}
