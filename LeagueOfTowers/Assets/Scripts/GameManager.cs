using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    //fields
    //this is for choosing tower from sidebar
    [SerializeField] private TowerButton pickedButton;

    //these for currency display
    private int currency;
    [SerializeField] private Text currencyText;

    //reference to the StartWave button
    [SerializeField]
    private GameObject waveButton;

    //the current selected tower
    private Tower selectedTower;

    //the list of the monsters in the wave
    private List<Monster> activeMonsters = new List<Monster>();

    //counter for the Waves
    private int wave = 0;
    //holds the reference to the text that shows the number of waves
    [SerializeField]
    private Text waveText;
    
    //Pool of objects (monsters/towers)
    public ObjectPool Pool{ get; set; }

    //keeps the status of the wave(true = wave in process; false = not wave time)
    private bool WaveActive{
        get{
            return activeMonsters.Count > 0;
        }
    }

    private void Awake(){
        Pool = GetComponent<ObjectPool>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetCurrency(100);
        SetCurrencyText();
    }

    // Update is called once per frame
    void Update()
    {
        CancelPickedTower();
        
    }

    //methods related to currency
    private void SetCurrency(int value)
    {
        this.currency = value;
    }
    
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

    public void SelectTower(Tower tower)
    {
        //if tower exist, deselect it
        if (selectedTower != null)
        {
            selectedTower.Select();
        }

        selectedTower = tower;
        selectedTower.Select();
    }

    public void DeselectTower()
    {
        if (selectedTower != null)
        {
            selectedTower.Select();
        }

        selectedTower = null;
    }

    //method that starts a summoning parocess of the monster wave
    public void StartWave(){
        wave++;
        waveText.text = string.Format("Wave: <color=lime>{0}</color>", wave);
        StartCoroutine(SpawnWave());

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
        if(!WaveActive){
            waveButton.SetActive(true);
        }
    }
}
