using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LightTowerScript : TowerScript
{
    //header
    string tooltipHeader = "<Size=2><b>Light Tower</b></size>";

    protected override void Start()
    {
        base.Start();
        // base.Set();
        // setElementType(Element.LIGHT);

        // Upgrades = new TowerUpgrade[]
        // {
        //     //price, damage, debuffDuration, debuffProcChance
        //     new TowerUpgrade(10, 5, 1f, 5),
        //     new TowerUpgrade(15, 10, 1f, 5),
        // };
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        base.OnPhotonInstantiate(info); //done 0-10
        //Debug.Log(this.projectileType);
    }

    public override Debuff GetDebuff()
    {
        return new LightDebuff(GetTargetMonster(), getDebuffDuration());
    }

    //override and add string to default tower stats
    public override string GetStats()
    {
        //additional info
        string addInfo = "\nDeal low damage \nand stun monsters";

        //return the default string result
        string result = string.Format("<color=#FFFF00>{0}</color>{1} {2}",
            tooltipHeader, base.GetStats(), addInfo);

        return result;
    }

    //override and also add tower header string
    // public override string GetTechStats()
    // {
    //     //return a string result with tower header
    //     string result = string.Format("<color=#FFFF00>{0}</color> {1}", tooltipHeader, base.GetTechStats());

    //     return result;
    // }

    public override void Upgrade()
    {
        if(this.view.IsMine)
        {
            this.view.RPC("upgradeLightRPC", RpcTarget.All);
        }
    }

    //punRPC
    [PunRPC]
    private void upgradeLightRPC()
    {
        //decrease shared global currency
        if(PhotonNetwork.IsMasterClient){
            CurrencyManager.Instance.SubCurrency(this.nextUpgrade.Price);
        }
        
        //increase price of this tower
        this.price += this.nextUpgrade.Price;

        //increase the damage, debuff proc chance, and debuff duration of tower
        this.damage += this.nextUpgrade.Damage;
        this.debuffChance += this.nextUpgrade.DebuffProcChance;
        this.duration += this.nextUpgrade.DebuffDuration;

        //increase tower level
        this.level++;
        this.nextUpgradeLevel++;
        setNextUpgrade();
    }
}
