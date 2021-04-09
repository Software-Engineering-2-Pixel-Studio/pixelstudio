using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class GameOver : Singleton<GameOver>
{
    //fields
    [SerializeField] private Text wavesDisplay;     //display box of number of waves on the scene
    [SerializeField] private GameObject restartButton;  //restart button

    private PhotonView view;        //photonview object for synchronize

    // Start is called before the first frame update
    private void Start()
    {
        view = this.GetComponent<PhotonView>();
        wavesDisplay.text = string.Format("Survived {0} waves", WaveManager.Instance.GetWaves().ToString());

        //only MasterClient can restart the game
        if(PhotonNetwork.IsMasterClient == false){
            restartButton.SetActive(false);
        }
        else{
            WaveManager.Instance.StopWave();
            WaveManager.Instance.DeactiveMonsters();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    /*
        PUNRPC method
        Method to let all player re-load the Map scene
    */
    [PunRPC]
    private void Restart()
    {
        PhotonNetwork.LoadLevel("Map");
    }


    //public method
    /*
        Method to call Restart method and send the signal to all players
    */
    public void OnClickRestart()
    {
        view.RPC("Restart", RpcTarget.All);
    }
}
