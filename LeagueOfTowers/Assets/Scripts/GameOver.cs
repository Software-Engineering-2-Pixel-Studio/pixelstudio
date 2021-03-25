using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class GameOver : Singleton<GameOver>
{
    //fields
    [SerializeField] private Text wavesDisplay;
    [SerializeField] private GameObject restartButton;

    private PhotonView view;

    // Start is called before the first frame update
    private void Start()
    {
        view = this.GetComponent<PhotonView>();
        wavesDisplay.text = string.Format("Survived {0} waves", WaveManager.Instance.GetWaves().ToString());
        ;

        //only MasterClient can restart the game
        if(PhotonNetwork.IsMasterClient == false){
            restartButton.SetActive(false);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    [PunRPC]
    private void Restart()
    {
        PhotonNetwork.LoadLevel("Map");
    }


    //public method
    public void OnClickRestart()
    {
        view.RPC("Restart", RpcTarget.All);
    }
}
