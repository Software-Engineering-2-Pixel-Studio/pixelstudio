using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PointTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void PointTestsSimplePasses()
    {
        Point point = new Point(1, 1);
        //check if the created point was assigned correct values
        Assert.AreEqual(point.X, 1);
        Assert.AreEqual(point.Y, 1); 
    }

    [Test]
    public void arePointsEqualTests()
    {
        Point point = new Point(1, 1);  
        Point pointEqual = new Point(1, 1);
        Point pointNotEqual = new Point(1, 3);
        bool areEqual = Point.arePointsEqual(point, pointEqual);
        bool areNotEqual = Point.arePointsEqual(point, pointNotEqual);
        //check if the methods' results are as expected
        Assert.IsTrue(areEqual);
        Assert.IsFalse(areNotEqual);
    }
    
    [Test]
    public void calculateDifferenceTests()
    {
        Point pointOne = new Point(3, 3);  
        Point pointTwo = new Point(1, 1);
        Point pointResult = Point.calculateDifference(pointOne, pointTwo);
        Point expected = new Point(2, 2);
        //check if the methods' results are as expected
        Assert.AreEqual(pointResult, expected);
    }
}
