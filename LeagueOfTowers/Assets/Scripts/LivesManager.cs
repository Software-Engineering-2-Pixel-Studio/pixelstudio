using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class LivesManager : Singleton<LivesManager>
{
    //fields
    [SerializeField] private int lives;         //number of global share lives 
    [SerializeField] private Text livesDisplay; //display box for global lives on scene
    [SerializeField] private GameObject gameOver;   //GameOver Image object
    private PhotonView view;       //PhotonView object to synchronize
    // Start is called before the first frame update
    private void Start()
    {
        this.lives = 3;
        this.view = this.GetComponent<PhotonView>();
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    //PUN RPC methods
    /*
        a synchronize method for substract number of lives whenever
        an enemy reaches base.
    */
    [PunRPC]
    private void subLivesRPC()
    {
        if(--this.lives <= 0)
        {
            this.gameOver.SetActive(true);

        }
        this.livesDisplay.text = this.lives.ToString();
    }

    //public methods

    /*
        a method to call subLivesRPC and send signal to all players
    */
    public void SubLives()
    {
        this.view.RPC("subLivesRPC", RpcTarget.All);
    }

    /*
        Method to get global lives
    */
    public int GetLives()
    {
        return this.lives;
    }

}
