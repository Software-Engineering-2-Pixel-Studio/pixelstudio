using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class SlowTowerScript : TowerScript
{
    [SerializeField] private float slowFactor; //slow factor of tower

    //header
    string tooltipHeader = "<Size=2><b>Slow Tower</b></size>";

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
        object[] data = this.gameObject.GetPhotonView().InstantiationData;
        if(data != null && data.Length == 12)
        {
            this.slowFactor = (float) data[11];
        }
    }
    

    public float getSlowFactor()
    {
        return this.slowFactor;
    }

    public override Debuff GetDebuff()
    {
        return new SlowDebuff(slowFactor, getDebuffDuration(), GetTargetMonster());
    }

    //override and add string to default tower stats
    public override string GetStats()
    {
        //additional info
        string addInfo = "\nDeal low damage and \nslow monsters";

        //return the default string result
        string result = string.Format("<color=#00ffffff>{0}</color>{1} \nSlow Speed: {2}% {3}",
            tooltipHeader, base.GetStats(), slowFactor, addInfo);

        if (this.nextUpgrade != null) //if next upgrade is available
        {
            result = string.Format("<color=#00ffffff>{0}</color>{1} \nSlow Speed: {2}% <color=#00ff00ff>+{3}%</color> {4}",
                tooltipHeader, base.GetStats(), slowFactor, this.nextUpgrade.SlowingFactor, addInfo);
        }

        return result;
    }
    
    public override void Upgrade()
    {
        if(this.view.IsMine)
        {
            this.view.RPC("upgradeSlowRPC", RpcTarget.All);
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
            this.view.RPC("techUpgradeSlowRPC", RpcTarget.All);
        }
    }

    //punRPC
    [PunRPC]
    private void upgradeSlowRPC()
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

        this.slowFactor += this.nextUpgrade.SlowingFactor;

        //increase tower level
        this.level++;
        this.nextUpgradeLevel++;
        setNextUpgrade();
    }

    [PunRPC]
    private void techUpgradeSlowRPC()
    {
        this.upgradedTech = true;
        
        this.projectileSpeed += this.techUpgrade.ProjectileSpeed;
        this.attackCooldown += this.techUpgrade.AttackCoolDown;
    }


}
