using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BaseTowerScript : TowerScript
{
    //header
    string tooltipHeader = "<Size=2><b>Base Tower</b></size>";

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
}
