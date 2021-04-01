using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTower : Tower
{
    protected override void Start()
    {
        base.Set();
        setElementType(Element.LIGHT);
    }

    public override Debuff GetDebuff()
    {
        return new LightDebuff(getTarget(), getDebuffDuration());
    }
}
