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
        if(this.currency >= towerButton.GetPrice())
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
}
