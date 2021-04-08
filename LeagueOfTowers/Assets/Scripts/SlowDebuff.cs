using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SlowDebuff : Debuff
{
    //slow factor for speed
    private float slowFactor;

    //for checking if slow is applied
    private bool slowIsApplied;

    public SlowDebuff(float givenSlowFactor, float duration, MonsterScript target) : base(target, duration)
    {
        this.slowFactor = givenSlowFactor;
    }

    public override void Update()
    {
        //if debuff has a target
        if (target != null)
        {
            if (!slowIsApplied) //if slow is not applied
            {
                //apply speed and lower the speed.
                slowIsApplied = true;
                //float slowSpeed = (target.getMaxSpeed() * slowFactor) / 100;
                //target.decreaseSpeed(slowSpeed);
            }
        }

        base.Update();
    }

    public override void Remove()
    {
        if (target != null) //if target exists
        {
            slowIsApplied = false;

            //set speed back to normal max speed
            //target.setSpeed(target.getMaxSpeed());

            base.Remove();
        }
    }
}