using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechUpgrade
{
    public float ProjectileSpeed { get; private set; }

    public float AttackCoolDown { get; private set; }

    public TechUpgrade(float projectileSpeed, float attackCoolDown)
    {
        this.ProjectileSpeed = projectileSpeed;
        this.AttackCoolDown = attackCoolDown;
    }
}
