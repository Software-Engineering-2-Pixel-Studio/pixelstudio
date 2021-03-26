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
        //if tower exist, deselect it
        if (selectedTower != null)
        {
            selectedTower.Select();
        }

        selectedTower = tower;
        selectedTower.Select();

        sellPriceText.text = (selectedTower.getPrice() / 2).ToString() + "<color='lime'>$</color>";

        //upgradePanel.SetActive(true);
        this.towerSelectPanel.SetActive(true);
    }

    /*
        Method to deselect tower and hide the select tower panel
    */
    public void DeselectTower()
    {
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
            //UpdateCurrency((selectedTower.getPrice()/2) + currency);
            CurrencyManager.Instance.AddCurrency(selectedTower.getPrice()/2);
            //Destroy(selectedTower.transform.parent.gameObject);
            //selectedTower.transform.parent.gameObject.SetActive(false);
            //DestroyTower(selectedTower.transform.parent.gameObject);
            //PhotonNetwork.Destroy(selectedTower.transform.parent.gameObject.GetComponent<Tower>().GetPhotonView());
            MapManager.Instance.SetTileIsEmptyAt(selectedTower.GetPlacedAtTile().X, selectedTower.GetPlacedAtTile().Y);
            selectedTower.DestroyThisTower();
            
            DeselectTower();
            
        }
    }
}
