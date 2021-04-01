using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerButton : MonoBehaviour
{
    //fields
    [SerializeField] private GameObject towerPrefab;        //tower's prefabs
    [SerializeField] private Sprite sprite;                 //tower's image

    [SerializeField] private int price; //tower's price
    [SerializeField] private Text priceText;    //display box for display tower's price on scene


    // Start is called before the first frame update
    private void Start()
    {
        this.SetPriceText();

        CurrencyManager.Instance.Changed += new CurrencyChanged(PriceCheck);
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    /*
        Method for Displaying the tower's price on scene
    */
    private void SetPriceText()
    {
        this.priceText.text = this.GetPrice().ToString() + "<color=lime>$</color>";
    }

    /*
        Method for getting tower's prefab
    */
    public GameObject GetTowerPrefab()
    {
        return this.towerPrefab;
    }

    /*
        Method for getting tower's image
    */
    public Sprite GetSprite()
    {
        return this.sprite;
    }

    /*
        Method for getting tower's price
    */
    public int GetPrice()
    {
        return this.price;
    }

    private void PriceCheck()
    {
        if (price <= CurrencyManager.Instance.GetCurrency())
        {
            GetComponent<Image>().color = Color.white;

            priceText.color = Color.white;
        }
        else
        {
            GetComponent<Image>().color = Color.gray;

            priceText.color = Color.gray;
        }
    }
    
}
