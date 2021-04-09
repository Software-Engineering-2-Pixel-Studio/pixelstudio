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
        Point point = new Point(1, 1);
        //check if the created point was assigned correct values
        Assert.AreEqual(point.X, 1);
        Assert.AreEqual(point.Y, 1);       
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator DebuffTestsWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
