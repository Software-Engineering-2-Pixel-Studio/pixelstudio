using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class ProjectileScript : MonoBehaviourPun, IPunInstantiateMagicCallback
{
    //fields
    private string myName;
    private float mySpeed;
    private float myDamage;
    private TowerScript ownerTower;
    private MonsterScript targetMonster;


    //components
    private PhotonView view;

    // Start is called before the first frame update
    void Start()
    {
        this.view = this.GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if(this.targetMonster != null ){
            //Debug.Log("moving");
            if(this.targetMonster.GetIsActive())
            {
                this.moveToTargetMonster();
            }
            else
            {
                DestroyThisProjectile();
            }
            
        }
        else{
            DestroyThisProjectile();
        }
        // else{
        //     Debug.Log("not found");
        // }
    }

    

    //get/set fields
    public TowerScript GetOwnerTower()
    {
        return this.ownerTower;
    }

    public int GetProjectileViewID()
    {
        return this.view.ViewID;
    }

    //events
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        //Debug.Log("called!");
        object[] data = this.gameObject.GetPhotonView().InstantiationData;
        if(data != null && data.Length == 5){
            //projectileType == projectileName
            this.myName = (string) data[0];
            //projectileSpeed
            this.mySpeed = (float) data[1];
            //projectileDamage
            this.myDamage = (float) data[2];
            //towerViewID
            if(PhotonView.Find((int) data[3]) != null){
                this.ownerTower = PhotonView.Find((int) data[3]).gameObject.GetComponent<TowerScript>();
            }
            //monsterViewID
            if(PhotonView.Find((int) data[4]) != null){
                this.targetMonster = PhotonView.Find((int) data[4]).gameObject.GetComponent<MonsterScript>();
            }
            
        }
    }

    private void ApplyDebuff()
    {
        
        //roll a chance number for debuff
        float rollNum = Random.Range(0, 101);

        //add the debuff if roll is more than or equal to proc chance
        if (rollNum <= ownerTower.getDebuffProcChance())
        {
            Debug.Log("Apply debuff");

            //get parent tower's debuff and apply it to target monster
            targetMonster.AddDebuff(ownerTower.GetDebuff());
        }
    }

    /*
        this method is called if projectile hit other ojbects
    */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Monster") //if the target in range is a monster
        {
            if(targetMonster == null){
                //Destroy(this.gameObject);
                //Debug.Log("Target Monster is not found");
                DestroyThisProjectile();
                return;
            }
            else {
                if (targetMonster.gameObject == other.gameObject)
                {
                    //Debug.Log("Monster hit");

                    if(this.view != null )
                    {
                        if(this.view.IsMine){
                            targetMonster.TakeDamage(this.myDamage);
                            ApplyDebuff();

                        }
                        DestroyThisProjectile();
                    }
                    // targetMonster.TakeDamage(this.myDamage);
                    // DestroyThisProjectile();
                    
                    
                }
            }

        }
    }

    //actions
    /*
        this method to move the projectile to target
    */
    private void moveToTargetMonster()
    {
        if(this.targetMonster.GetIsAlive())
        {
            //move from the origin into the target monster based on projectilespeed + ms of a frame
            this.transform.position = Vector3.MoveTowards(this.transform.position, this.targetMonster.transform.position, Time.deltaTime * mySpeed);
            
            Vector2 projectileDir = targetMonster.transform.position - transform.position;

            //get the angle needed to transform the projectile direction
            float angle = Mathf.Atan2(projectileDir.y, projectileDir.x) * Mathf.Rad2Deg;

            //rotate the projectile in the proper direction
            this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else{
            DestroyThisProjectile();
        }
    }

    /*
        this method to destroy this projectile
    */
    public void DestroyThisProjectile(){
        //only the owner can destroy this projectile
        if(this.view.IsMine)
        {
            PhotonNetwork.Destroy(this.transform.gameObject);
        }
    }
}
