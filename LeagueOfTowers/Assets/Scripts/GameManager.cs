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

    //this function will update the currency and currency display
    //whenever it changes
    public void UpdateCurrency(int value)
    {
        SetCurrency(value);
        SetCurrencyText();
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

    //get the amount of gold we have
    public int GetCurrency()
    {
        return this.currency;
    }

    //function to update the currency after placed a tower
    //ofc, this function is only call when we can pay for a tower
    public void PayForPlacedTower()
    {
        int newCurrency = this.currency - this.pickedButton.GetPrice();
        this.UpdateCurrency(newCurrency);

        this.pickedButton = null;
    }

    public void StartWave(){
        wave++;
        waveText.text = string.Format("Wave: <color=lime>{0}</color>", wave);
        StartCoroutine(SpawnWave());

        waveButton.SetActive(false);
    }

    private IEnumerator SpawnWave(){
        MapManager.Instance.GeneratePath();
        for(int i = 0; i < wave; i++){
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

    public void removeMonster(Monster monster){
        activeMonsters.Remove(monster);
        if(!WaveActive){
            waveButton.SetActive(true);
        }
    }
}
