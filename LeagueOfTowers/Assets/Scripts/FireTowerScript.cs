using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FireTowerScript : TowerScript
{
    //tick damage and tick time of fire debuff
    [SerializeField] private float tickTime;
    [SerializeField] private float tickDamage;


    //header
    string tooltipHeader = "<Size=2><b>Fire Tower</b></size>";

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public void setTickTime(float givenTickTime)
    {
        this.tickTime = givenTickTime;
    }

    public void setTickDamage(float givenTickDamage)
    {
        this.tickDamage = givenTickDamage;
    }

    

    public float getTickTime()
    {
        return this.tickTime;
    }

    public float getTickDamage()
    {
        return this.tickDamage;
    }

    //set/get

    //events
    public override void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        base.OnPhotonInstantiate(info); //done 0-10
        //Debug.Log(this.projectileType);
        object[] data = this.gameObject.GetPhotonView().InstantiationData;
        if(data != null && data.Length == 13)
        {
            this.tickDamage = (float) data[11];
            this.tickTime = (float) data[12];
        }
    }
    //actions

    //punRPC

    public override Debuff GetDebuff()
    {
        return new FireDebuff(tickDamage, tickTime, getDebuffDuration(), GetTargetMonster());
    }

    //override and add string to default tower stats
    public override string GetStats()
    {
        //additional info
        string addInfo = "\nDeal damage with a \nchance of lingering \ndamage over time";

        //return the default string result
        string result = string.Format("<color=#ffa500ff>{0}</color>{1} \nTick time: {2}sec \nTick damage: {3} {4}",
            tooltipHeader, base.GetStats(), tickTime, tickDamage, addInfo);

        if (this.nextUpgrade != null) //if next upgrade is available
        {
            result = string.Format("<color=#ffa500ff>{0}</color>{1} \nTick time: {2}sec <color=#00ff00ff>+{4}</color> \nTick damage: {3}<color=#00ff00ff>+{5}</color> {6}",
                tooltipHeader, base.GetStats(), tickTime, tickDamage, this.nextUpgrade.TickTime, this.nextUpgrade.TickDamage, addInfo);
        }

        return result;
    }

    public override void Upgrade()
    {
        if(this.view.IsMine)
        {
            this.view.RPC("upgradeFireRPC", RpcTarget.All);
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
            this.view.RPC("techUpgradeFireRPC", RpcTarget.All);
        }
    }

    //punRPC
    [PunRPC]
    private void upgradeFireRPC()
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

        this.tickDamage += this.nextUpgrade.TickDamage;
        this.tickTime += this.nextUpgrade.TickTime;

        //increase tower level
        this.level++;
        this.nextUpgradeLevel++;
        setNextUpgrade();
    }

    [PunRPC]
    private void techUpgradeFireRPC()
    {
        this.upgradedTech = true;
        
        this.projectileSpeed += this.techUpgrade.ProjectileSpeed;
        this.attackCooldown += this.techUpgrade.AttackCoolDown;
    }
}
