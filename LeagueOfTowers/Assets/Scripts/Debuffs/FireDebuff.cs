using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDebuff : Debuff
{
    //tick time, damage and time since last tick
    private float tickDamage;
    private float tickTIme; //how often debuff will tick
    private float timeSinceLastTick;

    public FireDebuff(float givenTickDamage, float givenTickTime, float duration, Monster target) : base(target, duration)
    {
        this.tickDamage = givenTickDamage;
        this.tickTIme = givenTickTime;
    }

    public override void Update()
    {
        //if debuff has a target
        if (target != null)
        {
            timeSinceLastTick += Time.deltaTime;

            //once the time is greater than/equal to the tick time
            if (timeSinceLastTick >= tickTIme) 
            {
                //reset timer for last tick
                timeSinceLastTick = 0;

                //do damage to monster
                target.TakeDamage(tickDamage, Element.FIRE);
            }
        }
        base.Update();
    }
    
    //************************************************************
    //GETTERS AND SETTERS
    //************************************************************
    /*
        Method to get the tickDamage
    */
    public float getTickDamage()
    {
        return this.tickDamage;
    }
    /*
        Method to get the tickTIme
    */
    public float getTickTIme()
    {
        return this.tickTIme;
    }
    /*
        Method to get the timeSinceLastTick
    */
    public float getTimeSinceLastTick()
    {
        return this.timeSinceLastTick;
    }
}
