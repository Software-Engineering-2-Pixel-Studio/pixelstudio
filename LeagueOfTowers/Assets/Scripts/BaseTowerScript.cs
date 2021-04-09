using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BaseTowerScript : TowerScript
{
    //header
    //string tooltipHeader = "<Size=2><b>Base Tower</b></size>";

    protected override void Start()
    {
        base.Start();
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

    // public override Debuff GetDebuff()
    // {
    //     return new LightDebuff(getTarget(), getDebuffDuration());
    // }

    //override and add string to default tower stats
    // public override string GetStats()
    // {
    //     //additional info
    //     string addInfo = "\nDeal low damage \nand stun monsters";

    //     //return the default string result
    //     string result = string.Format("<color=#FFFF00>{0}</color>{1} {2}",
    //         tooltipHeader, base.GetStats(), addInfo);

    //     return result;
    // }

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
            this.view.RPC("upgradeBaseRPC", RpcTarget.All);
        }
    }

    public override void TechUpgrade()
    {
        //only owner using this
        if(this.view.IsMine)
        {   
            //only this owner's token is reduce
            LevelUpManager.Instance.SpendToken();

            //send signal to other player about this tower's stat change
            this.view.RPC("techUpgradeBaseRPC", RpcTarget.All);
        }
    }

    //punRPC
    [PunRPC]
    private void upgradeBaseRPC()
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

        // this.slowFactor += this.nextUpgrade.SlowingFactor;
        // Debug.Log("slowFactor of Slow tower = " + slowFactor);

        //increase tower level
        this.level++;
        this.nextUpgradeLevel++;
        setNextUpgrade();
    }

    [PunRPC]
    private void techUpgradeBaseRPC()
    {
        this.upgradedTech = true;
        
        this.projectileSpeed += this.techUpgrade.ProjectileSpeed;
        this.attackCooldown += this.techUpgrade.AttackCoolDown;
    }
}
