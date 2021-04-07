using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class GameManager : Singleton<GameManager>
{
    //fields
    //this is for choosing tower from sidebar
    [SerializeField] private TowerButton pickedButton;


    //the list of the monsters in the wave
    //private List<Monster> activeMonsters = new List<Monster>();

    
    //Pool of objects (monsters/towers)
    public ObjectPool Pool{ get; set; }

    // reference to the game over menu object
    [SerializeField]
    private GameObject gameOverMenu;

    private void Awake(){
        Pool = GetComponent<ObjectPool>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        //this.view = GetComponent<PhotonView>();
        //Lives = 10;
    }

    // Update is called once per frame
    private void Update()
    {
        CancelPickedTower();
    }

    /*
        Method to catch mouse event (right click) for canceling
        the picked tower and also disable its hover image
    */
    private void CancelPickedTower()
    {
        //right-click to cancel
        if (Input.GetMouseButtonDown(1))
        {
            //deactive the sripte image of Hover
            Hover.Instance.Deactivate();

            //reset picked button
            this.pickedButton = null;
        }
    }

    //get the picked tower button
    public TowerButton GetPickedTowerButton()
    {
        return this.pickedButton;
    }

    //this function is called when user click on the button from the panel
    public void SetPickedTower(TowerButton towerButton)
    {
        this.pickedButton = towerButton;
        
    }

    //function to update the currency after placed a tower
    //ofc, this function is only call when we can pay for a tower
    public void PayForPlacedTower()
    {
        CurrencyManager.Instance.SubCurrency(this.pickedButton.GetPrice());
        this.pickedButton = null;
    }

    //quits the game
    //      -> implementation of the functionality of the Quit button
    public void QuitGame(){
        Application.Quit();
    }

}
