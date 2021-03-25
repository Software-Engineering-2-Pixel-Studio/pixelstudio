using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    //to control the coroutine of the wave
    Coroutine waveCoroutine;

    //fields
    //this is for choosing tower from sidebar
    [SerializeField] private TowerButton pickedButton;

    //these for currency display
    private int currency;
    [SerializeField] private Text currencyText;

    //variables to control players' lives
    private int lives;
    [SerializeField]
    private Text livesText;

    //reference to the StartWave button
    [SerializeField]
    private GameObject waveButton;

    //the list of the monsters in the wave
    private List<Monster> activeMonsters = new List<Monster>();

    //counter for the Waves
    private int wave = 0;
    //holds the reference to the text that shows the number of waves
    [SerializeField]
    private Text waveText;
    
    //Pool of objects (monsters/towers)
    public ObjectPool Pool{ get; set; }

    //determines if the game is ended or not
    private bool gameOver = false;

    // reference to the game over menu object
    [SerializeField]
    private GameObject gameOverMenu;

    //keeps the status of the wave(true = wave in process; false = not wave time)
    private bool WaveActive{
        get{
            return activeMonsters.Count > 0;
        }
    }

    //sets the number of lives to the value and the text of the lives
    //      Also returns the value of the lives
    public int Lives{
        get{
            return lives;
        }
        set{
            if (lives == 1){
                this.lives = 0;
                GameOver();
            }
            this.lives = value;
            this.livesText.text = this.lives.ToString();
        }
    }
    private void Awake(){
        Pool = GetComponent<ObjectPool>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetCurrency(100);
        Lives = 10;
    }

    // Update is called once per frame
    void Update()
    {
        CancelPickedTower();
    }

    //sets the currency to the value
    private void SetCurrency(int value)
    {
        this.currency = value;
        SetCurrencyText();
    }
    //sets the text of the currency
    private void SetCurrencyText()
    {
        this.currencyText.text = this.currency.ToString() + "<color=lime>$</color>";
    }
   
    //this function will update the currency and currency display
    //whenever it changes
    public void UpdateCurrency(int value)
    {
        SetCurrency(value);
        SetCurrencyText();
    }

    //get the amount of gold we have
    public int GetCurrency()
    {
        return this.currency;
    }

    //get the picked tower button
    public TowerButton GetPickedTowerButton()
    {
        return this.pickedButton;
    }

    //this function is called when user click on the button from the panel
    public void SetPickedTower(TowerButton towerButton)
    {
        //check if we have enough gold to choose this tower
        if(this.currency >= towerButton.GetPrice() && !WaveActive)
        {
            //set current picked button
            this.pickedButton = towerButton;

            //activate the Hover's SpriteRenderer and set its Sprite to the one from the button.
            Hover.Instance.Activate(this.pickedButton.GetSprite());
        }
        
    }

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

    //function to update the currency after placed a tower
    //ofc, this function is only call when we can pay for a tower
    public void PayForPlacedTower()
    {
        int newCurrency = this.currency - this.pickedButton.GetPrice();
        this.UpdateCurrency(newCurrency);

        this.pickedButton = null;
    }

    //method that starts a summoning parocess of the monster wave
    public void StartWave(){
        wave++;
        waveText.text = string.Format("Wave: <color=lime>{0}</color>", wave);
        waveCoroutine = StartCoroutine(SpawnWave());

        waveButton.SetActive(false);    // disactivate a button until wave is done
    }

    // method that spawns a wave of monsters to come out
    private IEnumerator SpawnWave(){
        int numberOfMonster = wave * 3; // every wave the numver of the monsters will be increase by 3
        for(int i = 0; i < numberOfMonster; i++){
            MapManager.Instance.GeneratePath(); //regenerate a new path for every monster
            int monsterIndex = Random.Range(0, 2);
            string type = string.Empty;
            switch (monsterIndex)
            {
                case 0:
                    type = "TrainingDummy";
                    break;
                case 1:
                    type = "Scarecrow";
                    break;
                default:
                    break;
            }
            Monster monster = Pool.GetObject(type).GetComponent<Monster>();
            monster.Spawn();

            activeMonsters.Add(monster);

            yield return new WaitForSeconds(2.5f);
        }
    }

    // removes a monster from the list of the monsters on the field
    //      -> if there are no monsters left activates the ability to start next Wave
    public void removeMonster(Monster monster){
        activeMonsters.Remove(monster);
        if(!WaveActive && !gameOver){
            waveButton.SetActive(true);
        }
    }

    // finishes the game
    //      -> implementation of the game over functionality
    public void GameOver(){
        if(!gameOver){
            gameOver = true;
            //stop producing new monsters on the map
            StopCoroutine(waveCoroutine);
            //stop existing monsters on the field from moving and remove them
            for(int i=0; i<=activeMonsters.Count; i++){
                activeMonsters[i].SetActive(false);
                activeMonsters.Remove(activeMonsters[i]);
            }
            activeMonsters[0].SetActive(false);
            activeMonsters.Remove(activeMonsters[0]);
            //activate the game over screen
            gameOverMenu.SetActive(true);
        }
    }

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
}
