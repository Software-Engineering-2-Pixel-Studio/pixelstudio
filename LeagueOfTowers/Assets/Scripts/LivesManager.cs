using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class LivesManager : Singleton<LivesManager>
{
    //fields
    [SerializeField] private int lives;

    [SerializeField] private Text livesDisplay;

    [SerializeField] private GameObject gameOver;
    private PhotonView view;
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
            //if(PhotonNetwork.IsMasterClient)
            //GameManager.Instance.GameOver();
            this.gameOver.SetActive(true);

        }
        this.livesDisplay.text = this.lives.ToString();
    }

    //public methods

    /*
        a method to substract lives
    */
    public void SubLives()
    {
        this.view.RPC("subLivesRPC", RpcTarget.All);
    }

    public int GetLives()
    {
        return this.lives;
    }

// public int Lives{
//         get{
//             return lives;
//         }
//         set{
//             if (lives == 1){
//                 this.lives = 0;
//                 GameOver();
//             }
//             this.lives = value;
//             this.livesText.text = this.lives.ToString();
//         }
//     }

}
