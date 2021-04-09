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

    //for stats panel
    [SerializeField] private GameObject statsPanel;
    [SerializeField] private Text statText;
    [SerializeField] private Text sellPriceText;    //selling price of the tower
    [SerializeField] private Text upgradePriceText;     //upgrading price of the tower

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
    public void SelectTower(TowerScript tower)
    {
        //Debug.Log("clicked");
        if (sTower != null)
        {
            sTower.Select();
        }

        sTower = tower;
        sTower.Select();

        sellPriceText.text = (sTower.GetPrice() / 2).ToString() + "<color='lime'>$</color>";
        if (sTower.GetNextUpgrade() != null)
        {
            upgradePriceText.text = sTower.GetNextUpgrade().Price.ToString() + "<color='lime'>$</color>";
        }
        else
        {
            //if there are no more upgrades, just make the string empty
            upgradePriceText.text = "<color='lime'>MaxLevel</color>";
        }

        this.towerSelectPanel.SetActive(true);
    }

    /*
        Method to deselect tower and hide the select tower panel
    */
    public void DeselectTower()
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
    public void SellTower()
    {
        //Debug.Log("Sell Tower 2 is called");
        if(sTower != null)
        {
            
            CurrencyManager.Instance.AddCurrency(sTower.GetPrice() / 2);

            MapManager.Instance.SetTileIsPlacedAt2(sTower.GetParentTileID(), false);

            sTower.DestroyThisTower();

            DeselectTower();
        }
    }

    /*
        This method will be called when the UpgradeButton has been clicked
    */
    public void UpgradeTower()
    {
        if (sTower != null)
        {
            //if the current tower level is lower than the number of upgrades available
            //and if the current shared global currency is bigger than the price for upgrade
            if (sTower.GetLevel() < 3 )
            {
                if(sTower.GetNextUpgrade() == null)
                {
                    Debug.Log("Cant found nextUpgrade");
                    return;
                }
                else if(CurrencyManager.Instance.GetCurrency() >= sTower.GetNextUpgrade().Price)
                {
                    sTower.Upgrade();
                    UpdateUpgradeTooltip();
                    
                }
            }
        }
    }

    /*
        This method shows the selected tower stats
    */
    public void ShowSelectedTowerStats()
    {
        statsPanel.SetActive(true);
        UpdateUpgradeTooltip();
    }

    public void HideSelectedTowerStats()
    {
        statsPanel.SetActive(false);
        UpdateUpgradeTooltip();
    }

    /*
        This method updates the tooltip for upgrade
    */
    public void UpdateUpgradeTooltip()
    {
        if (sTower != null)
        {
            //update the text of sell button
            sellPriceText.text = (sTower.GetPrice() / 2).ToString() + "<color='lime'>$</color>";

            //update the tooltip text
            SetToolTipText(sTower.GetStats());

            //upgrade the price text
            if (sTower.GetNextUpgrade() != null)
            {
                upgradePriceText.text = sTower.GetNextUpgrade().Price.ToString() + "<color='lime'>$</color>";
            }
            else
            {
                //if there are no more upgrades, just make the string empty
                upgradePriceText.text = "<color='lime'>MaxLevel</color>";;
            }
        }
    }

    /*
        This method sets the text for the hover upgrade tooltip
    */
    public void SetToolTipText(string givenText)
    {
        statText.text = givenText;
    }
}
