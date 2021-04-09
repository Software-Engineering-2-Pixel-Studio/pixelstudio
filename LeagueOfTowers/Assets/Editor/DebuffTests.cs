using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class DebuffTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void DebuffTestsSimplePasses()
    {
        // Debuff class vars
        Monster targetMonsterLight = new Monster();
        float duration = 1.5f;
        
        //create an instance of every type of tower upgrade
        //Monster target, float duration
        LightDebuff debuffLight = new LightDebuff( targetMonsterLight, duration);

        //Check that all of the variables are set properly
        Assert.AreEqual(debuffLight.getDuration(), duration);
        Assert.AreEqual(debuffLight.getTarget(), targetMonsterLight);   
    }

    [Test]
    public void FireDebuffTests()
    {
        // Debuff class vars
        Monster targetMonsterFire = new Monster();
        float duration = 1.5f;
        // for Fire Debuff
        float givenTickDamage = 1.5f;
        float givenTickTime = 0.5f; //how often debuff will tick

        //create an instance of every type of tower upgrade
        //float givenTickDamage, float givenTickTime, float duration, Monster target
        FireDebuff debuffFire = new FireDebuff(givenTickDamage, givenTickTime, duration, targetMonsterFire);

        //Check that all of the variables are set properly
        Assert.AreEqual(debuffFire.getDuration(), duration);
        Assert.AreEqual(debuffFire.getTarget(), targetMonsterFire);
        Assert.AreEqual(debuffFire.getTickDamage(), givenTickDamage);
        Assert.AreEqual(debuffFire.getTickTIme(), givenTickTime);
    }

    [Test]
    public void SlowDebuffTests()
    {
        // Debuff class vars
        Monster targetMonsterSlow = new Monster();
        float duration = 1.5f;
        // for Slow Debuff
        float givenSlowFactor = 0.4f;

        //create an instance of every type of tower upgrade
        //float givenSlowFactor, float duration, Monster target
        SlowDebuff debuffSlow = new SlowDebuff( givenSlowFactor, duration, targetMonsterSlow);

        //Check that all of the variables are set properly
        Assert.AreEqual(debuffSlow.getDuration(), duration);
        Assert.AreEqual(debuffSlow.getTarget(), targetMonsterSlow);
        Assert.AreEqual(debuffSlow.getSlowFactor(), givenSlowFactor);
    }
}
