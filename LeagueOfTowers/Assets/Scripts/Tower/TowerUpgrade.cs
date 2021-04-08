using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerUpgrade
{
    public int Price { get; private set; }

    public int Damage { get; private set; }

    public float DebuffDuration { get; private set; }

    public float DebuffProcChance { get; private set; }

    public float SlowingFactor { get; private set; }

    public float TickTime { get; private set; }

    public int TickDamage { get; private set; }

    //simple or light tower
    public TowerUpgrade(int price, int damage, float debuffDuration, float debuffProcChance)
    {
        Price = price;
        Damage = damage;
        DebuffDuration = debuffDuration;
        DebuffProcChance = debuffProcChance;
    }

    //fire tower
    public TowerUpgrade(int price, int damage, float debuffDuration, float debuffProcChance, float tickTime, int tickDamage)
    {
        Price = price;
        Damage = damage;
        DebuffDuration = debuffDuration;
        DebuffProcChance = debuffProcChance;
        TickTime = tickTime;
        TickDamage = tickDamage;
    }

    //slow tower
    public TowerUpgrade(int price, int damage, float debuffDuration, float debuffProcChance, float slowingFactor)
    {
        Price = price;
        Damage = damage;
        DebuffDuration = debuffDuration;
        DebuffProcChance = debuffProcChance;
        SlowingFactor = slowingFactor;
    }
}
