using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class SelectTowerManager : Singleton<SelectTowerManager>
{
    //fields
    private TowerScript sTower;
    [SerializeField ] private GameObject towerSelectPanel;  //upgrade panel of tower
    [SerializeField] private Text sellPriceText;    //selling price of the tower

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    /*
        Method to select tower and show up the select tower panel
    */
    public void SelectTower2(TowerScript tower)
    {
        if (sTower != null)
        {
            sTower.Select();
        }

        sTower = tower;
        sTower.Select();

        sellPriceText.text = (sTower.GetPrice() / 2).ToString() + "<color='lime'>$</color>";

        this.towerSelectPanel.SetActive(true);
    }

    /*
        Method to deselect tower and hide the select tower panel
    */
    public void DeselectTower2()
    {
        if(sTower != null)
        {
            sTower.Select();
        }

        this.sTower = null;
        this.towerSelectPanel.SetActive(false);
    }

    /*
        This method will be called when the SellButton has been clicked
    */
    public void SellTower2()
    {
        Debug.Log("Sell Tower 2 is called");
        if(sTower != null)
        {
            
            CurrencyManager.Instance.AddCurrency(sTower.GetPrice() / 2);

            MapManager.Instance.SetTileIsPlacedAt2(sTower.GetParentTileID(), false);

            sTower.DestroyThisTower();

            DeselectTower2();
        }
    }
}
