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
    public int X { get; set; }

    public int Y { get; set; }

    public Point(int x, int y){
        this.X = x;
        this.Y = y;
    }
}
