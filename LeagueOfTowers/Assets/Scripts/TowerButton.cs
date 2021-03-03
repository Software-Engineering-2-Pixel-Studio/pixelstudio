using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerButton : MonoBehaviour
{
    //fields
    //these for image display of the button
    [SerializeField] private GameObject towerPrefab;
    [SerializeField] private Sprite sprite;

    //this for show the price of the button
    [SerializeField] private int price;
    [SerializeField] private Text priceText;


    // Start is called before the first frame update
    void Start()
    {
        this.SetPriceText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetPriceText()
    {
        this.priceText.text = this.GetPrice().ToString() + "<color=lime>$</color>";
    }

    public GameObject GetTowerPrefab()
    {
        return this.towerPrefab;
    }

    public Sprite GetSprite()
    {
        return this.sprite;
    }

    public int GetPrice()
    {
        return this.price;
    }

    
}
