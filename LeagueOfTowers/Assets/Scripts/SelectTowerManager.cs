using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class SelectTowerManager : MonoBehaviour
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
}
