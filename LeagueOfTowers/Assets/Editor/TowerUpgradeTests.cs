using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TowerUpgradeTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void TowerUpgradeTestsSimplePasses()
    {
        int price = 10;
        int damage = 15;
        float debuffDuration = 1.5f;
        float debuffProcChance = 0.2f;

        //create an instance of every type of tower upgrade
        //int price, int damage, float debuffDuration, float debuffProcChance
        TowerUpgrade upgradeLight = new TowerUpgrade( price, damage, debuffDuration, debuffProcChance);
        
        //Check that all of the variables are set properly
        //Light
        Assert.AreEqual(upgradeLight.Price, 10);  
        Assert.AreEqual(upgradeLight.Damage, 15);
        Assert.AreEqual(upgradeLight.DebuffDuration, 1.5f);
        Assert.AreEqual(upgradeLight.DebuffProcChance, 0.2f);
    }

    [Test]
    public void FireTowerTests()
    {
        int price = 10;
        int damage = 15;
        float debuffDuration = 1.5f;
        float debuffProcChance = 0.2f;
        float tickTime = 1.3f;
        int tickDamage = 3;

        //create an instance of every type of tower upgrade
        //int price, int damage, float debuffDuration, float debuffProcChance, float tickTime, int tickDamage
        TowerUpgrade upgradeFire = new TowerUpgrade(price, damage, debuffDuration, debuffProcChance, tickTime, tickDamage);
    
        //Check that all of the variables are set properly
        //Fire
        Assert.AreEqual(upgradeFire.Price, 10);
        Assert.AreEqual(upgradeFire.Damage, 15);
        Assert.AreEqual(upgradeFire.DebuffDuration, 1.5f);
        Assert.AreEqual(upgradeFire.DebuffProcChance, 0.2f);
        Assert.AreEqual(upgradeFire.TickTime, 1.3f);
        Assert.AreEqual(upgradeFire.TickDamage, 3f);
    }

    [Test]
    public void SlowTowerTests()
    {
        int price = 10;
        int damage = 15;
        float debuffDuration = 1.5f;
        float debuffProcChance = 0.2f;
        float slowingFactor = 0.4f;

        //create an instance of every type of tower upgrade
        //int price, int damage, float debuffDuration, float debuffProcChance, float slowingFactor
        TowerUpgrade upgradeSlow = new TowerUpgrade( price, damage, debuffDuration, debuffProcChance, slowingFactor);
    
        //Check that all of the variables are set properly
        //Slow
        Assert.AreEqual(upgradeSlow.Price, 10);
        Assert.AreEqual(upgradeSlow.Damage, 15);
        Assert.AreEqual(upgradeSlow.DebuffDuration, 1.5f);
        Assert.AreEqual(upgradeSlow.DebuffProcChance, 0.2f);
        Assert.AreEqual(upgradeSlow.SlowingFactor, 0.4f);
    }
}
