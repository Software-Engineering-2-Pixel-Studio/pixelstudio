using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTower : Tower
{
    [SerializeField] private float slowFactor; //slow factor of tower

    protected override void Start()
    {
        base.Set();
        setElementType(Element.SLOW);
    }

    public override Debuff GetDebuff()
    {
        return new SlowDebuff(slowFactor, getDebuffDuration(),getTarget());
    }

    public float getSlowFactor()
    {
        return this.slowFactor;
    }
}
