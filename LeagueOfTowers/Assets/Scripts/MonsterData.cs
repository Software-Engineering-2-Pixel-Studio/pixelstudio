using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterData : Singleton<MonsterData>
{
    private const int propertyCount = 7;
    public static object[] GetDummyData()
    {
        object[] data = new object[propertyCount];
        //name
        data[0] = (string) "TrainingDummy";
        //base health
        data[1] = (float) 10.0f;
        //speed
        data[2] = (float) 3.0f;
        //income
        data[3] = (int) 4;
        //isAlive
        data[4] = (bool) true;
        //isActive
        data[5] = (bool) true;
        //monsterEXP
        data[6] = (int) 25;
        return data;
    }
    
    public static object[] GetScarecrowData()
    {
        object[] data = new object[propertyCount];
        //name
        data[0] = (string) "Scarecrow";
        //base health
        data[1] = (float) 20.0f;
        //speed
        data[2] = (float) 3.0f;
        //income
        data[3] = (int) 8;
        //isAlive
        data[4] = (bool) true;
        //isActive
        data[5] = (bool) true;
        //EXP
        data[6] = (int) 50;
        return data;
    }
}
