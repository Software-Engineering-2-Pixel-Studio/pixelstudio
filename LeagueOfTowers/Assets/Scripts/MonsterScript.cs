using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MonsterScript : MonoBehaviourPun, IPunInstantiateMagicCallback
{
    private string myName;
    [SerializeField] private float myHP;
    private float mySpeed;
    private int myIncome;
    private bool isAlive;
    private bool isActive;

    private Stack<Node> path;
    private Point GridPosition{ get; set;}
    private Vector3 destination; // the destination of the monster (base location)

    private PhotonView view;
    // Start is called before the first frame update
    void Start()
    {
        this.view = this.GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if(this.isActive){
             move();
        }
    }

    //get/set
    public bool GetIsAlive()
    {
        return this.isAlive;
    }

    public bool GetIsActive()
    {
        return this.isActive;
    }

    public int GetMonsterViewID()
    {
        return this.view.ViewID;
    }
    private void setPath(Stack<Node> newPath){
        if(newPath != null){
            this.path = newPath;
            this.GridPosition = this.path.Peek().GridPosition;
            this.destination = this.path.Pop().WorldPosition;
        }
    }
    private void move(){
        if(this.isActive){
            this.transform.position = Vector2.MoveTowards(this.transform.position, this.destination, this.mySpeed*Time.deltaTime);

            if(this.transform.position == this.destination){
                if(this.path != null && this.path.Count > 0){
                    this.GridPosition = path.Peek().GridPosition;
                    this.destination = path.Pop().WorldPosition;
                }
            }
        }
    }
    
    //events
    /*
        initialize this Monster script's fields when it is created over network
    */
    public void OnPhotonInstantiate(PhotonMessageInfo info) //replace spawn
    {
        //Debug.Log("Monster is created!");
        object[] data = this.gameObject.GetPhotonView().InstantiationData;
        if(data != null && data.Length == 6){
            //name
            this.myName = (string) data[0];
            //hp
            this.myHP = (float) data[1];
            //monsterSpeed
            this.mySpeed = (float) data[2];
            //monsterIncome
            this.myIncome = (int) data[3];
            //isAlive
            this.isAlive = (bool) data[4];
            //isActive
            this.isActive = (bool) data[5];

            //scale animation when spawn
            if(this.myName == "TrainingDummy"){
                StartCoroutine(Scale(new Vector3(0.1f, 0.1f), new Vector3(0.6f,0.6f), false));
            }
            else if(this.myName == "Scarecrow"){
                StartCoroutine(Scale(new Vector3(0.1f, 0.1f), new Vector3(0.5f,0.5f), false));
            }

            setPath(MapManager.Instance.Path);
        }
    }

    /*
        coroutine for scaling animation
    */
    public IEnumerator Scale(Vector3 from, Vector3 to, bool destroy)
    {
        float progress = 0.0f;

        while (progress<=1)
        {
            this.transform.localScale = Vector3.Lerp(from, to, progress);
            progress += Time.deltaTime;
            yield return null;
        }
        
        //to make sure it is exactly equal to *to*
        this.transform.localScale = to;

        //in case we need to release the moster and make it inactive
        if(destroy){
            //Release();
            ChangeIsActiveState(false);
            ChangeIsAliveState(false);
            WaveManager.Instance.RemoveMonster(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //if the monster collide with the base
        if(other.tag == "BasePortal"){
            Debug.Log("Reach the base");
            //scale down in size them
            if(this.myName == "TrainingDummy"){
                StartCoroutine(Scale(new Vector3(0.6f, 0.6f), new Vector3(0.1f,0.1f), true));
            }
            else if(this.myName == "Scarecrow"){
                StartCoroutine(Scale(new Vector3(0.5f, 0.5f), new Vector3(0.1f,0.1f), true));
            }

            //decrease players' lives 
            LivesManager.Instance.SubLives();
        }
    }

    //action
    /*
        Method that reduce the health of this monster when it collide with the projectile
        also, increase the global currency
    */
    public void TakeDamage(float damage)
    {
        if (this.isAlive && this.isActive) //if monster is active and alive
        {
            //do some damage
            //this.myHP -= damage;
            DecreaseHP(damage);
            //Debug.Log("health: " + healthValue.ToString());

            if (this.myHP <= 0) //if it's dead (health is 0)
            {
                CurrencyManager.Instance.AddCurrency(this.myIncome);

                //this.isActive = false;
                ChangeIsActiveState(false);
                ChangeIsAliveState(false);

                WaveManager.Instance.RemoveMonster(this);
            }
        }
    }
    
    //punRPC methods for synchronzing this script's fields over the network
    [PunRPC]
    private void changeIsActiveStateRPC(bool state)
    {
        this.isActive = state;
    }

    public void ChangeIsActiveState(bool state)
    {
        this.view.RPC("changeIsActiveStateRPC", RpcTarget.All, state);
    }

    [PunRPC]
    private void changeIsAliveStateRPC(bool state)
    {
        this.isAlive = state;
    }
    public void ChangeIsAliveState(bool state)
    {
        this.view.RPC("changeIsAliveStateRPC", RpcTarget.All, state);
    }

    [PunRPC]
    private void decreaseHPRPC(float damage)
    {
        this.myHP -= damage;
    }
    public void DecreaseHP(float damage)
    {
        this.view.RPC("decreaseHPRPC", RpcTarget.All, damage);
    }

    [PunRPC]
    private void destroyThisMonsterRPC()
    {
        PhotonNetwork.Destroy(this.gameObject);
    }

    public void DestroyThisMonster()
    {
        this.view.RPC("destroyThisMonsterRPC", RpcTarget.MasterClient);
    }

}
