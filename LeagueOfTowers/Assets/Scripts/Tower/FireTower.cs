using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTower : Tower
{
    //tick damage and tick time of fire debuff
    [SerializeField] private float tickTime;
    [SerializeField] private float tickDamage;

    protected override void Start()
    {
        base.Set();
        setElementType(Element.FIRE);
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
}
