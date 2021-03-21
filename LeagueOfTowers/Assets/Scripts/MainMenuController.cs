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
    [SerializeField] private GameObject UserNameCanvas;
    [SerializeField] private GameObject MainMenuCanvas;

    [SerializeField] private InputField UserNameInputField;
    [SerializeField] private InputField CreateInputField;
    [SerializeField] private InputField JoinInputField;

    [SerializeField] private GameObject OKButton;


    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // Start is called before the first frame update
    private void Start()
    {
        UserNameCanvas.SetActive(true);
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
        if(UserNameInputField.text.Length >= 3)
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
        UserNameCanvas.SetActive(false);
        PhotonNetwork.NickName = UserNameInputField.text;
    }

    public void CreateGame()
    {
        //PhotonNetwork.CreateRoom(CreateGameInput.text, new RoomOptions() { MaxPlayers = Constants.MAXPLAYERS }, null);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = Constants.MAXPLAYERS;
        PhotonNetwork.CreateRoom(CreateInputField.text, roomOptions);
    }

    public void JoinGame()
    {
        //PhotonNetwork.JoinOrCreateRoom(JoinGameInput.text, new RoomOptions() { MaxPlayers = Constants.MAXPLAYERS }, TypedLobby.Default);
        PhotonNetwork.JoinRoom(JoinInputField.text);
    }

    // public override void OnConnectedToMaster()
    // {
    //     PhotonNetwork.JoinLobby(TypedLobby.Default);
    //     Debug.Log("Connected");
    // }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Map");
    }

}
