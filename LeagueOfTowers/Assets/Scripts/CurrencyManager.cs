using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CurrencyManager : Singleton<CurrencyManager>
{
    //fields
    [SerializeField] private int currency;
    [SerializeField] private Text currencyDisplay;

    private PhotonView view;
    // Start is called before the first frame update
    private void Start()
    {
        this.currency = 500;
        this.currencyDisplay.text = string.Format("{0}<color='lime'>$</color>", this.currency);
        this.view = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    //PUNRPC methods
    /*
        a synchronize method for adding an amount to the currency when
        an enemy is killed
    */
    [PunRPC]
    private void addCurrencyRPC(int earnAmount)
    {
        this.currency += earnAmount;
        this.currencyDisplay.text = this.currency.ToString();
    }

    /*
        a synchronize method for subtracting an amount to the currency
        when pay for a tower.
    */
    [PunRPC]
    private void subCurrencyRPC(int payAmount)
    {
        this.currency -= payAmount;
        this.currencyDisplay.text = this.currency.ToString();
    }

    //public methods
    public void AddCurrency(int earnAmount)
    {
        this.view.RPC("addCurrencyRPC", RpcTarget.All, earnAmount);
    }

    public void SubCurrency(int payAmount)
    {
        this.view.RPC("subCurrencyRPC", RpcTarget.All, payAmount);
    }

    public int GetCurrency(){
        return this.currency;
    }


}
