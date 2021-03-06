using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class PlayerController : MonoBehaviour
{
    //fields
    //[SerializeField] private float speed;
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private GameObject playerCanvas;
    [SerializeField] private Text playerNameDisplay;
    private PhotonView view;


    // Start is called before the first frame update
    private void Start()
    {
        //this.speed = 10;
        this.view = this.GetComponent<PhotonView>();
        if(this.view.IsMine)
        {
            this.transform.GetComponent<PlayerMovement>().enabled = true;
            this.playerCamera.GetComponent<Camera>().enabled = true;
            this.playerCamera.GetComponent<AudioListener>().enabled = true;
            this.playerCanvas.GetComponent<Canvas>().enabled = true;
            this.playerNameDisplay.GetComponent<Text>().enabled = true;
            if(PhotonNetwork.IsMasterClient){
                this.playerNameDisplay.text = string.Format("<color='white'>{0}</color>",PhotonNetwork.NickName);
            }
            else{
                this.playerNameDisplay.text = string.Format("<color='blue'>{0}</color>",view.Owner.NickName);
            }
            
        }
        else
        {
            this.transform.GetComponent<PlayerMovement>().enabled = false;
            this.playerCamera.GetComponent<Camera>().enabled = false;
            this.playerCamera.GetComponent<AudioListener>().enabled = false;
            this.playerCanvas.GetComponent<Canvas>().enabled = false;
            this.playerNameDisplay.GetComponent<Text>().enabled = false;
            if(PhotonNetwork.IsMasterClient){
                this.playerNameDisplay.text = string.Format("<color='white'>{0}</color>", PhotonNetwork.NickName);
            }
            else{
                this.playerNameDisplay.text = string.Format("<color='blue'>{0}</color>",view.Owner.NickName);
            }
            
            
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // if(view.IsMine){
        //     //if these are not specific who is the local player, all players on scene will
        //     //move when anyone hit the keyboard
        //     //get input from keyboard
        //     Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        //     //speed * Time.deltaTime = distance -> Find the destination from a distance which direction
        //     Vector2 moveAmount = moveInput.normalized * speed * Time.deltaTime;

        //     //Actually move to the destination on scene
        //     transform.position += (Vector3)moveAmount;
        // }
    }
}
