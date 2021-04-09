using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Debuff
{
    //targeted monster of debuff
    protected MonsterScript target;

    //debuff duration and time elapsed since last debuff
    private float duration;
    private float elapsed;


    //debuff constructor
    public Debuff(MonsterScript otherTarget, float givenDuration)
    {
        this.target = otherTarget;
        this.duration = givenDuration;
    }

    //update function for debuff
    public virtual void Update()
    {
        //get time per seconds
        elapsed += Time.deltaTime;

        //if time elapsed is equal or more than duration
        if (elapsed-1.5f >= duration)
        {
            //remove the debuff
            Remove();
        }
    }

    //remove debuff
    public virtual void Remove()
    {
        if (target != null) //if target exists
        {
            target.RemoveDebuff(this);
        }
    }
}