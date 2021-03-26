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
    //fields
    [SerializeField] private string GameVersion = "1.0";    //Game version
    [SerializeField] private GameObject UserNameCanvas;     //Canvas for getting UserName input
    [SerializeField] private GameObject MainMenuCanvas;     //Canvas for create and join room

    [SerializeField] private InputField UserNameInputField;   //the input field for username
    [SerializeField] private InputField CreateInputField;   //the input field for create room
    [SerializeField] private InputField JoinInputField;     //the input field for join room

    [SerializeField] private GameObject OKButton;       //the Ok button


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

    /*
        Method to get this game version
    */
    public string GetGameVersion()
    {
        return GameVersion;
    }

    /*
        Method to make the Ok Button appear when there are at least 3 character in the input field
    */
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

    /*
        Method is called when OK button has been clicked which
        set the user name for this player
    */
    public void SetUserName()
    {
        UserNameCanvas.SetActive(false);
        PhotonNetwork.NickName = UserNameInputField.text;
    }

    /*
        Method is called when Create Button has been clicked which
        create a new room that this player is the host
    */
    public void CreateGame()
    {
        //PhotonNetwork.CreateRoom(CreateGameInput.text, new RoomOptions() { MaxPlayers = Constants.MAXPLAYERS }, null);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = Constants.MAXPLAYERS;
        PhotonNetwork.CreateRoom(CreateInputField.text, roomOptions);
    }

    /*
        Method is called when the Join Button has been clicked which
        join a room that this player is a client
    */
    public void JoinGame()
    {
        //PhotonNetwork.JoinOrCreateRoom(JoinGameInput.text, new RoomOptions() { MaxPlayers = Constants.MAXPLAYERS }, TypedLobby.Default);
        PhotonNetwork.JoinRoom(JoinInputField.text);
    }

    /*
        Load the "Map" scene when join a room
    */
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Map");
    }

}
