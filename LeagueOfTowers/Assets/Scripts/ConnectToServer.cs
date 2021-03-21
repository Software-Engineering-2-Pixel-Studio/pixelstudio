using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        
    }



    // Update is called once per frame
    private void Update()
    {
        
    }

    public override void OnConnectedToMaster(){
        SceneManager.LoadScene("MainMenu");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("PUN : OnDisconnected() was called by PUN with reason {0}", cause);
    }
}