using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTower : Tower
{
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

    public override string GetStats()
    {
        //header
        string tooltipHeader = "<Size=2><b>Light Tower</b></size>";

        //additional info
        string addInfo = "\nDeal low damage \nand stun monsters";

        //return the default string result
        string result = string.Format("<color=#FFFF00>{0}</color>{1} {2}",
            tooltipHeader, base.GetStats(), addInfo);

        return result;
    }
}
