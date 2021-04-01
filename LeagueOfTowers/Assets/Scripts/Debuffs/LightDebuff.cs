using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDebuff : Debuff
{
    public LightDebuff(Monster target, float duration) : base(target, duration)
    {

    }

    public override void Update()
    {
        //if there's a target
        if (target != null)
        {
            target.setSpeed(0);
        }

        base.Update();
    }

    //for removing debuffs
    public override void Remove()
    {
        if (target != null) //if target exists
        {
            //set speed to original max speed of target
            target.setSpeed(target.getMaxSpeed());

            base.Remove();
        }

        
    }
}
