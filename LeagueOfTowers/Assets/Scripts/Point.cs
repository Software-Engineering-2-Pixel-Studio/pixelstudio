using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//different between a struct and a class
//is when you pass a struct into a function
//function will create a local variable of this struct
//so nothing is saved into the real variable of struct
//after the function finished.
public struct Point
{
    public int X { get; set; }          //X coordinate

    public int Y { get; set; }          //Y coordinate

    public Point(int x, int y) {
        this.X = x;
        this.Y = y;
    }

    /*
        Methods to check if 2 Point are equal
    */
    public static bool arePointsEqual(Point firstPoint, Point secondPoint)
    {
        return firstPoint.X == secondPoint.X && firstPoint.Y == secondPoint.Y;
    }

    /*
        Method to calculate the difference between 2 Point (vector)
    */
    public static Point calculateDifference(Point firstPoint, Point secondPoint)
    {
        return new Point(firstPoint.X - secondPoint.X, firstPoint.Y - secondPoint.Y);
    }
}
