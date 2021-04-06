using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTower : Tower
{
    //header
    string tooltipHeader = "<Size=2><b>Light Tower</b></size>";

    protected override void Start()
    {
        base.Set();
        setElementType(Element.LIGHT);

        Upgrades = new TowerUpgrade[]
        {
            //price, damage, debuffDuration, debuffProcChance
            new TowerUpgrade(10, 5, 1f, 5),
            new TowerUpgrade(15, 10, 1f, 5),
        };
    }

    public override Debuff GetDebuff()
    {
        return new LightDebuff(getTarget(), getDebuffDuration());
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
    public override string GetTechStats()
    {
        //return a string result with tower header
        string result = string.Format("\n<color=#FFFF00>{0}</color> {1}", tooltipHeader, base.GetTechStats());

        return result;
    }
}
