using System;
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
        //float debuffDuration = 1.5;
        //float debuffProcChance = 0.2;
        //create an instance of every type of tower upgrade
        //int price, int damage, float debuffDuration, float debuffProcChance
        //TowerUpgrade upgradeLight = new TowerUpgrade( price, damage, debuffDuration, debuffProcChance);
        //int price, int damage, float debuffDuration, float debuffProcChance, float tickTime, int tickDamage
        //TowerUpgrade upgradeFire = new TowerUpgrade( 10, 15, 1.5, 0.2, 1.3, 3);
        //int price, int damage, float debuffDuration, float debuffProcChance, float slowingFactor
        //TowerUpgrade upgradeSlow = new TowerUpgrade( 10, 15, 1.5, 0.2, 0.4);

        //Check that all of the variables are set properly
        //Light
        //Assert.AreEqual(upgradeLight.Price, 10);
/*  
        Assert.AreEqual(upgradeLight.Damage, 15);
        Assert.AreEqual(upgradeLight.DebuffDuration, 1.5);
        Assert.AreEqual(upgradeLight.DebuffProcChance, 0.2);

        //Fire
        Assert.AreEqual(upgradeFire.Price, 10);
        Assert.AreEqual(upgradeFire.Damage, 15);
        Assert.AreEqual(upgradeFire.DebuffDuration, 1.5);
        Assert.AreEqual(upgradeFire.DebuffProcChance, 0.2);
        Assert.AreEqual(upgradeFire.TickTime, 1.3);
        Assert.AreEqual(upgradeFire.TickDamage, 3);

        //Slow
        Assert.AreEqual(upgradeSlow.Price, 10);
        Assert.AreEqual(upgradeSlow.Damage, 15);
        Assert.AreEqual(upgradeSlow.DebuffDuration, 1.5);
        Assert.AreEqual(upgradeSlow.DebuffProcChance, 0.2);
        Assert.AreEqual(upgradeSlow.SlowingFactor, 0.4);
        */
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator TowerUpgradeTestsWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
