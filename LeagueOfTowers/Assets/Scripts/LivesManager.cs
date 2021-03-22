using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class LivesManager : MonoBehaviour
{
    //fields
    [SerializeField] private int lives;

    [SerializeField] private Text livesDisplay;

    private PhotonView view;
    // Start is called before the first frame update
    private void Start()
    {
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
        this.lives--;
        this.livesDisplay.text = this.lives.ToString();
    }

    //public methods

    /*
        a method to substract lives
    */
    public void subLives()
    {
        this.view.RPC("subLivesRPC", RpcTarget.All);
    }
}
