using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTower : Tower
{
    [SerializeField] private float slowFactor; //slow factor of tower

    //header
    string tooltipHeader = "<Size=2><b>Slow Tower</b></size>";

    protected override void Start()
    {
        base.Set();
        setElementType(Element.SLOW);

        Upgrades = new TowerUpgrade[]
        {
            //price, damage, debuffDuration, debuffProcChance, slowing factor
            new TowerUpgrade(10, 5, 1f, 5, 10),
            new TowerUpgrade(15, 10, 1f, 5, 20),
        };
    }

    public override Debuff GetDebuff()
    {
        return new SlowDebuff(slowFactor, getDebuffDuration(),getTarget());
    }

    public float getSlowFactor()
    {
        return this.slowFactor;
    }

    //override and add string to default tower stats
    public override string GetStats()
    {
        //additional info
        string addInfo = "\nDeal low damage and \nslow monsters";

        //return the default string result
        string result = string.Format("<color=#00ffffff>{0}</color>{1} \nSlow Speed: {2}% {3}",
            tooltipHeader, base.GetStats(), slowFactor, addInfo);

        if (GetNextUpgrade != null) //if next upgrade is available
        {
            result = string.Format("<color=#00ffffff>{0}</color>{1} \nSlow Speed: {2}% <color=#00ff00ff>+{3}%</color> {4}",
                tooltipHeader, base.GetStats(), slowFactor, GetNextUpgrade.SlowingFactor, addInfo);
        }

        return result;
    }

    //override and also add tower header string
    public override string GetTechStats()
    {
        //return a string result with tower header
        string result = string.Format("<color=#00ffffff>{0}</color> {1}", tooltipHeader, base.GetTechStats());

        return result;
    }

    //override the default upgrade tower function
    public override void Upgrade()
    {
        //increase the slowing factor
        this.slowFactor += GetNextUpgrade.SlowingFactor;
        base.Upgrade();
    }
}
