using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickedTowerScript : MonoBehaviour
{
    [SerializeField] private TowerButton pickedButton;
    [SerializeField] private GameObject playerCamera;
    // private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        // if(this.pickedButton != null){
        //     //hover sprite
        // }
    }

    //public method
    public void SetPickedButton(TowerButton clickedTowerButton){
        
        //Debug.Log("Click Button " + clickedTowerButton.GetTowerPrefab().name);
        if(CurrencyManager.Instance.GetCurrency() >= clickedTowerButton.GetPrice()){
            this.pickedButton = clickedTowerButton;

            GameManager.Instance.SetPickedTower(this.pickedButton);
            Hover.Instance.Activate(this.pickedButton.GetSprite(), playerCamera.GetComponent<Camera>());
        }
        
    }

    public TowerButton GetPickedButton(){
        return this.pickedButton;
    }
}
