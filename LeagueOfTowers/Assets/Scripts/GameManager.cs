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

    //the current selected tower
    private Tower selectedTower;

    //upgrade panel of tower
    [SerializeField ] private GameObject upgradePanel;

    //selling price of the tower
    [SerializeField] private Text sellPriceText;

    //the list of the monsters in the wave
    private List<Monster> activeMonsters = new List<Monster>();

    
    //Pool of objects (monsters/towers)
    public ObjectPool Pool{ get; set; }

    //determines if the game is ended or not
    //private bool gameOver = false;

    // reference to the game over menu object
    [SerializeField]
    private GameObject gameOverMenu;

    //private PhotonView view;

    
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
        // //check if we have enough gold to choose this tower
        //if(this.currency >= towerButton.GetPrice() && !WaveActive)
        // {
        //     //set current picked button
        //     this.pickedButton = towerButton;

        //     //activate the Hover's SpriteRenderer and set its Sprite to the one from the button.
        //     //Hover.Instance.Activate(this.pickedButton.GetSprite());
        // }
        this.pickedButton = towerButton;
        
    }

    //function to update the currency after placed a tower
    //ofc, this function is only call when we can pay for a tower
    public void PayForPlacedTower()
    {
        CurrencyManager.Instance.SubCurrency(this.pickedButton.GetPrice());
        this.pickedButton = null;
    }

    public void SelectTower(Tower tower)
    {
        //if tower exist, deselect it
        if (selectedTower != null)
        {
            selectedTower.Select();
        }

        selectedTower = tower;
        selectedTower.Select();

        sellPriceText.text = "Sell for " + (selectedTower.GetPrice() / 2).ToString();

        upgradePanel.SetActive(true);
    }

    public void DeselectTower()
    {
        if (selectedTower != null)
        {
            selectedTower.Select();
        }

        selectedTower = null;
        upgradePanel.SetActive(false);
    }

    //method that starts a summoning parocess of the monster wave
    // public void StartWave(){
    //     wave++;
    //     waveText.text = string.Format("Wave: <color=lime>{0}</color>", wave);
    //     waveCoroutine = StartCoroutine(SpawnWave());

    //     waveButton.SetActive(false);    // disactivate a button until wave is done
    // }

    // method that spawns a wave of monsters to come out
    // private IEnumerator SpawnWave(){
    //     int numberOfMonster = wave * 3; // every wave the numver of the monsters will be increase by 3
    //     for(int i = 0; i < numberOfMonster; i++){
    //         MapManager.Instance.GeneratePath(); //regenerate a new path for every monster
    //         int monsterIndex = Random.Range(0, 2);
    //         string type = string.Empty;
    //         switch (monsterIndex)
    //         {
    //             case 0:
    //                 type = "TrainingDummy";
    //                 break;
    //             case 1:
    //                 type = "Scarecrow";
    //                 break;
    //             default:
    //                 break;
    //         }
    //         Monster monster = Pool.GetObject(type).GetComponent<Monster>();
    //         monster.Spawn();

    //         activeMonsters.Add(monster);

    //         yield return new WaitForSeconds(2.5f);
    //     }
    // }

    // // removes a monster from the list of the monsters on the field
    // //      -> if there are no monsters left activates the ability to start next Wave
    // public void removeMonster(Monster monster){
    //     activeMonsters.Remove(monster);
    //     if(!WaveActive && !gameOver){
    //         waveButton.SetActive(true);
    //     }
    // }

    // finishes the game
    //      -> implementation of the game over functionality
    // public void GameOver(){
    //     if(!gameOver){
    //         gameOver = true;
    //         //stop producing new monsters on the map
    //         StopCoroutine(waveCoroutine);
    //         //stop existing monsters on the field from moving and remove them
    //         for(int i=0; i<=activeMonsters.Count; i++){
    //             activeMonsters[i].SetActive(false);
    //             activeMonsters.Remove(activeMonsters[i]);
    //         }
    //         activeMonsters[0].SetActive(false);
    //         activeMonsters.Remove(activeMonsters[0]);
    //         //activate the game over screen
    //         gameOverMenu.SetActive(true);
    //     }
    // }

    //restarts the game
    //      -> implementation of the functionality of the Restart button
    public void Restart(){
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //quits the game
    //      -> implementation of the functionality of the Quit button
    public void QuitGame(){
        Application.Quit();
    }

    public void SellTower()
    {
        if (selectedTower != null)
        {
            selectedTower.GetComponentInParent<Tile>().SetIsPlaced(false);
            //UpdateCurrency((selectedTower.getPrice()/2) + currency);
            CurrencyManager.Instance.AddCurrency(selectedTower.GetPrice()/2);
            //Destroy(selectedTower.transform.parent.gameObject);
            selectedTower.transform.parent.gameObject.SetActive(false);
            //DestroyTower(selectedTower.transform.parent.gameObject);
            //PhotonNetwork.Destroy(selectedTower.transform.parent.gameObject.GetComponent<Tower>().GetPhotonView());
            DeselectTower();
        }
    }

    // [PunRPC]
    // private void destroyTowerRPC(GameObject go){
    //     Destroy(go);
    // }

    // public void DestroyTower(GameObject gameObject){
    //     this.view.RPC("destroyTowerRPC", RpcTarget.All, gameObject);
    // }
}
