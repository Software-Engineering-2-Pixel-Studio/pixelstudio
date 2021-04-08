using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Monster : MonoBehaviour
{
    //fields
    private float healthValue;     //health of monster

    public bool isAlive { get { return healthValue > 0; } } //condition to check if monster is alive

    //money you get from killing these monsters
    private int dummyIncome = 5;
    private int scarecrowIncome = 10;

    //exp you get from killing these monsters
    private int expDummy = 25;
    private int expScarecrow = 40;

    [SerializeField]
    private float speed; //serialized to access from different places
    private Stack<Node> path;

    private float maxSpeed; //max speed of monster

    [SerializeField]  private Element elementType; //element type of the monster

    private int typeResistance = 2; //normal resistance of monster against its own type

    //list of debuffs to monsters
    private List<Debuff> debuffsList = new List<Debuff>();

    //debuffs added and removed to monster
    private List<Debuff> debuffsToAdd = new List<Debuff>();
    private List<Debuff> debuffsToRemove = new List<Debuff>();
    

    public Point GridPosition{ get; set;}

    private Vector3 destination; // the destination of the monster (base location)

    public bool IsActive{ get; set; }   // the condition of the monster (can  move or not)

    private void Awake()
    {
        maxSpeed = speed;
    }

    private void Update(){
        HandleDebuffs();
        Move();
    }

    public void SetActive(bool value){
        IsActive = value;
    }

    public void setSpeed(float givenSpeed)
    {
        this.speed = givenSpeed;
    }

    public void decreaseSpeed(float givenSpeed)
    {
        this.speed -= givenSpeed;
    }

    public void setMaxSpeed(float givenMax)
    {
        this.maxSpeed = givenMax;
    }

    //Spawns a monster on a map by setting it's position first to the position of the 
    //   Spawn portal
    public void Spawn(int health){

        if(this.name == "TrainingDummy"){
            StartCoroutine(Scale(new Vector3(0.1f, 0.1f), new Vector3(0.6f,0.6f), false));
            transform.position = MapManager.Instance.SpawnPrefab.transform.position;
            this.healthValue = health;
        }
        else if(this.name == "Scarecrow"){
            StartCoroutine(Scale(new Vector3(0.1f, 0.1f), new Vector3(0.5f,0.5f), false));
            transform.position = MapManager.Instance.SpawnPrefab.transform.position;
            this.healthValue = health*2;
        }

        SetPath(MapManager.Instance.Path);
    }

    //method that scales the size of the monster
    //used to create an illusion that they enter/appear from the base/spawn place
    public IEnumerator Scale(Vector3 from, Vector3 to, bool destroy){
        //IsActive = false;

        float progress = 0;

        while (progress<=1)
        {
            transform.localScale = Vector3.Lerp(from, to, progress);
            progress += Time.deltaTime;
            yield return null;
        }
        
        //to make sure it is exactly equal to *to*
        transform.localScale = to;

        IsActive = true;

        //in case we need to release the moster and make it inactive
        if(destroy){
            Release();
        }
    }

    //method to move the moster from their position towards the base
    private void Move(){
        if(IsActive){
            transform.position = Vector2.MoveTowards(transform.position, destination, speed*Time.deltaTime);

            if(transform.position == destination){
                if(path != null && path.Count > 0){
                    GridPosition = path.Peek().GridPosition;
                    destination = path.Pop().WorldPosition;
                }
            }
        }
    }

    //method that sets the path(from spawn to base) to the monster
    private void SetPath(Stack<Node> newPath){
        if(newPath != null){
            this.path = newPath;
            GridPosition = path.Peek().GridPosition;
            destination = path.Pop().WorldPosition;
        }
    }

    //method that starts action when the monster collides with the base 
    //      -> meaning that the monster enters the base
    private void OnTriggerEnter2D(Collider2D other){
        //if the monster collide with the base
        if(other.tag == "BasePortal"){
            //Debug.Log("Reach the base");
            //scale down in size them
            if(this.name == "TrainingDummy"){
                
                StartCoroutine(Scale(new Vector3(0.6f, 0.6f), new Vector3(0.1f,0.1f), true));
            }
            else if(this.name == "Scarecrow"){
                StartCoroutine(Scale(new Vector3(0.5f, 0.5f), new Vector3(0.1f,0.1f), true));
            }

            //decrease players' lives 
            LivesManager.Instance.SubLives();
        }
    }

    // method that releases the monster 
    //      -> sets as inactive and removes from the map to be seen, but leaves the object to be used again for the future
    private void Release(){

        //clear debuffs
        debuffsList.Clear();

        IsActive = false; // so next time we use the object it starts as not active;
        GridPosition = MapManager.Instance.SpawnPos; // to make sure next time we use the object it starts at start position
        WaveManager.Instance.GetPool().ReleaseObject(gameObject); // makes an object inactive for later usage
        WaveManager.Instance.removeMonster(this);   // removes the monster from the "active monsters of the wave" list
    }

    /*
        Method that reduce the health of this monster when it collide with the projectile
        also, increase the global currency
    */
    public void TakeDamage(float damage, Element dmgType)
    {
        if (IsActive) //if monster is active
        {
            //if damage type is the same as the monster type, do less damage
            if (dmgType == elementType)
            {
                damage = damage / typeResistance;
            }

            //do some damage
            healthValue -= damage;
            //Debug.Log("health: " + healthValue.ToString());

            //Debug.Log("speed: " + speed.ToString());
            //Debug.Log("Max speed: " + maxSpeed.ToString());

            if (healthValue <= 0) //if it's dead (health is 0)
            {;

                //add some currency and exp points depending on type of monster
                if (this.name == "TrainingDummy")
                {
                    CurrencyManager.Instance.AddCurrency(dummyIncome);
                    GameManager.Instance.addExp(expDummy);
                }
                else if (this.name == "Scarecrow")
                {
                    CurrencyManager.Instance.AddCurrency(scarecrowIncome);
                    GameManager.Instance.addExp(expScarecrow);
                }

                IsActive = false;

                //remove the monster from the pool of objects in scene
                WaveManager.Instance.GetPool().ReleaseObject(gameObject);
                WaveManager.Instance.removeMonster(this);
            }
        }
    }

    public void AddDebuff(Debuff givenDebuff)
    {
        //check the list of debuff if this given debuff already exist
        if (!debuffsList.Exists(debuff => debuff.GetType() == givenDebuff.GetType()))
        {
            //add debuff to the list
            debuffsToAdd.Add(givenDebuff);
        }
        
    }

    public void RemoveDebuff(Debuff givenDebuff)
    {
        //remove given debuff from list
        debuffsToRemove.Add(givenDebuff);
    }

    private void HandleDebuffs()
    {
        //if a debuff was added to the list (so more than zero)
        if (debuffsToAdd.Count > 0)
        {
            //add it to list of debuffs
            debuffsList.AddRange(debuffsToAdd);

            //make sure to clear the list
            debuffsToAdd.Clear();
        }

        //run through each debuff needed to remove
        foreach (Debuff debuff in debuffsToRemove)
        {
            //remove them from the list of debuffs
            debuffsList.Remove(debuff);
        }

        //clear the list of debuffs
        debuffsToRemove.Clear();

        foreach (Debuff debuff in debuffsList)
        {
            //update every debuff in the list
            debuff.Update();
        }
    }

    /*
        Method to get element type of this monster
    */
    public Element getElementType()
    {
        return this.elementType;
    }

    /*
        Method to get speed of this monster
    */
    public float getSpeed()
    {
        return this.speed;
    }

    public float getMaxSpeed()
    {
        return this.maxSpeed;
    }
}
