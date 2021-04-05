using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class SelectTowerManager : Singleton<SelectTowerManager>
{
    //fields
    private Tower selectedTower; //the current selected tower
    [SerializeField ] private GameObject towerSelectPanel;  //upgrade panel of tower
    [SerializeField] private Text sellPriceText;    //selling price of the tower

    //for stats panel
    [SerializeField] private GameObject statsPanel;
    [SerializeField] private Text statText;

    //for upgrade button
    [SerializeField] private Text upgradePrice;

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
    public void SelectTower(Tower tower)
    {
        //if tower exist
        if (selectedTower != null)
        {
            selectedTower.Select();
        }

        selectedTower = tower;
        selectedTower.Select();

        //update sell price
        sellPriceText.text = (selectedTower.getPrice() / 2).ToString() + " <color='lime'>$</color>";

        this.towerSelectPanel.SetActive(true);
    }

    /*
        Method to deselect tower and hide the select tower panel
    */
    public void DeselectTower()
    {
        //if tower exist
        if (selectedTower != null)
        {
            selectedTower.Select();
        }

        this.selectedTower = null;
        this.towerSelectPanel.SetActive(false);
    }

    /*
        This method will be called when the SellButton has been clicked
    */
    public void SellTower()
    {
        if (selectedTower != null)
        {
            
            selectedTower.GetComponentInParent<Tile>().SetIsPlaced(false);
    
            //add earned currency for selling tower to global currency
            CurrencyManager.Instance.AddCurrency(selectedTower.getPrice()/2);
            
            //set tile is empty
            MapManager.Instance.SetTileIsEmptyAt(selectedTower.GetPlacedAtTile().X, selectedTower.GetPlacedAtTile().Y);
            
            //destroy this tower
            selectedTower.DestroyThisTower();
            
            //un select this tower and hide the UI
            DeselectTower();    
        }
    }

    /*
        This method will be called when the UpgradeButton has been clicked
    */
    public void UpgradeTower()
    {
        if (selectedTower != null)
        {
            //if the current tower level is lower than the number of upgrades available
            //and if the current shared global currency is bigger than the price for upgrade
            if (selectedTower.getTowerLevel() <= selectedTower.Upgrades.Length && CurrencyManager.Instance.GetCurrency() >= selectedTower.GetNextUpgrade.Price)
            {
                selectedTower.Upgrade();
            }
        }
    }

    /*
        This method will be called when the UpgradeTechButton has been clicked
    */
    public void UpgradeTowerTech()
    {
        if (selectedTower != null)
        {
            if (selectedTower.getTowerLevel() <= selectedTower.Upgrades.Length && GameManager.Instance.getTechTokens() >= selectedTower.GetNextUpgrade.Price)
            {
                selectedTower.TechUpgrade();
            }
        }
    }

    /*
        This method shows the stats panel of the tower
    */
    public void ShowStats()
    {
        statsPanel.SetActive(!statsPanel.activeSelf);
    }

    /*
        This method shows the selected tower stats
    */
    public void ShowSelectedTowerStats()
    {
        statsPanel.SetActive(!statsPanel.activeSelf);
        UpdateUpgradeTooltip();
    }

    /*
        This method sets the text for the hover upgrade tooltip
    */
    public void SetToolTipText(string givenText)
    {
        statText.text = givenText;
    }

    /*
        This method updates the tooltip for upgrade
    */
    public void UpdateUpgradeTooltip()
    {
        if (selectedTower != null)
        {
            //update the text of sell button
            sellPriceText.text = (selectedTower.getPrice() / 2).ToString() + " <color='lime'>$</color>";

            //update the tooltip text
            SetToolTipText(selectedTower.GetStats());

            //upgrade the price text
            if (selectedTower.GetNextUpgrade != null)
            {
                upgradePrice.text = selectedTower.GetNextUpgrade.Price.ToString() + "<color='lime'>$</color>";
            }
            else
            {
                //if there are no more upgrades, just make the string empty
                upgradePrice.text = string.Empty;
            }
        }
    }
    
}
