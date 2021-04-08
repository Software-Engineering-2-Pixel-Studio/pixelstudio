using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerUpgrade
{
    public int Price { get; private set; }

    public float Damage { get; private set; }

    public float DebuffDuration { get; private set; }

    public float DebuffProcChance { get; private set; }

    public float SlowingFactor { get; private set; }

    public float TickTime { get; private set; }

    public float TickDamage { get; private set; }

    //base tower
    public TowerUpgrade(int price, float damage)
    {
        Price = price;
        Damage = damage;
    }
    
    //fire tower
    public TowerUpgrade(int price, float damage, float debuffDuration, float debuffProcChance, float tickTime, float tickDamage)
    {
        Price = price;
        Damage = damage;
        DebuffDuration = debuffDuration;
        DebuffProcChance = debuffProcChance;
        TickTime = tickTime;
        TickDamage = tickDamage;
    }

    //slow tower
    public TowerUpgrade(int price, float damage, float debuffDuration, float debuffProcChance, float slowingFactor)
    {
        Price = price;
        Damage = damage;
        DebuffDuration = debuffDuration;
        DebuffProcChance = debuffProcChance;
        SlowingFactor = slowingFactor;
    }

    //light tower
    public TowerUpgrade(int price, float damage, float debuffDuration, float debuffProcChance)
    {
        Price = price;
        Damage = damage;
        DebuffDuration = debuffDuration;
        DebuffProcChance = debuffProcChance;
    }
}
