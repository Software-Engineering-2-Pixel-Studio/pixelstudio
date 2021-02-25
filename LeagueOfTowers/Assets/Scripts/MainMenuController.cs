using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

static class Constants
{
    public const int MAXPLAYERS = 2;
}

public class MainMenuController : MonoBehaviourPunCallbacks
{
    
    [SerializeField] private string GameVersion = "1.0";
    [SerializeField] private GameObject UserNameMenuCanvas;
    [SerializeField] private GameObject MainMenuCanvas;

    [SerializeField] private InputField UserNameInput;
    [SerializeField] private InputField CreateGameInput;
    [SerializeField] private InputField JoinGameInput;

    [SerializeField] private GameObject OKButton;


    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // Start is called before the first frame update
    private void Start()
    {
        UserNameMenuCanvas.SetActive(true);
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = GameVersion;

    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    public string GetGameVersion()
    {
        return GameVersion;
    }

    public void ToggleOkButton()
    {
        if(UserNameInput.text.Length >= 3)
        {
            OKButton.SetActive(true);
        }
        else
        {
            OKButton.SetActive(false);
        }
    }

    public void SetUserName()
    {
        UserNameMenuCanvas.SetActive(false);
        PhotonNetwork.NickName = UserNameInput.text;
    }

    public void CreateGame()
    {
        PhotonNetwork.CreateRoom(CreateGameInput.text, new RoomOptions() { MaxPlayers = Constants.MAXPLAYERS }, null);
    }

    public void JoinGame()
    {
        PhotonNetwork.JoinOrCreateRoom(JoinGameInput.text, new RoomOptions() { MaxPlayers = Constants.MAXPLAYERS }, TypedLobby.Default);
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        Debug.Log("Connected");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("PUN : OnDisconnected() was called by PUN with reason {0}", cause);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Map");
    }

}
