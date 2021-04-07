using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTower : Tower
{
    //tick damage and tick time of fire debuff
    [SerializeField] private float tickTime;
    [SerializeField] private float tickDamage;

    //header
    string tooltipHeader = "<Size=2><b>Fire Tower</b></size>";

    protected override void Start()
    {
        base.Set();
        setElementType(Element.FIRE);

        Upgrades = new TowerUpgrade[]
        {
            //price, damage, debuffDuration, debuffProcChance, tickTime, damage per tick
            new TowerUpgrade(10, 5, 0.5f, 5, 0.1f, 1),
            new TowerUpgrade(15, 10, 0.5f, 5, 0.1f, 1),
        };
    }

    public void setTickTime(float givenTickTime)
    {
        this.tickTime = givenTickTime;
    }

    public void setTickDamage(float givenTickDamage)
    {
        this.tickDamage = givenTickDamage;
    }

    public override Debuff GetDebuff()
    {
        return new FireDebuff(tickDamage, tickTime, getDebuffDuration(), getTarget());
    }

    public float getTickTime()
    {
        return this.tickTime;
    }

    public float getTickDamage()
    {
        return this.tickDamage;
    }

    //override and add string to default tower stats
    public override string GetStats()
    {
        //additional info
        string addInfo = "\nDeal damage with a \nchance of lingering \ndamage over time";

        //return the default string result
        string result = string.Format("<color=#ffa500ff>{0}</color>{1} \nTick time: {2}sec \nTick damage: {3} {4}",
            tooltipHeader, base.GetStats(), tickTime, tickDamage, addInfo);

        if (GetNextUpgrade != null) //if next upgrade is available
        {
            result = string.Format("<color=#ffa500ff>{0}</color>{1} \nTick time: {2}sec <color=#00ff00ff>+{4}</color> \nTick damage: {3}<color=#00ff00ff>+{5}</color> {6}",
                tooltipHeader, base.GetStats(), tickTime, tickDamage, GetNextUpgrade.TickTime, GetNextUpgrade.TickDamage, addInfo);
        }

        return result;
    }

    //override and also add tower header string
    public override string GetTechStats()
    {
        //return a string result with tower header
        string result = string.Format("<color=#ffa500ff>{0}</color> {1}", tooltipHeader, base.GetTechStats());

        return result;
    }

    //override the default upgrade tower function
    public override void Upgrade()
    {
        //decrease ticktime and increase tick damage
        this.tickTime -= GetNextUpgrade.TickTime;
        this.tickDamage += GetNextUpgrade.TickDamage;
        base.Upgrade();
    }
}