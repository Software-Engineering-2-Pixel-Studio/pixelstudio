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

    /*
        After connect to Photon server, the scene will change to MainMenu scene
    */
    public override void OnConnectedToMaster(){
        SceneManager.LoadScene("MainMenu");
    }

    /*
        A case where the user cant connect to Photon Server.
    */
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("PUN : OnDisconnected() was called by PUN with reason {0}", cause);
    }
}