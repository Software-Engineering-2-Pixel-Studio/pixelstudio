using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerData : Singleton<TowerData>
{
    private const int propertyCount = 7;
    public static object[] getFireTowerData()
    {
        object[] data = new object[propertyCount];
        //name
        data[0] = (string) "FireTower";
        //price
        data[1] = (int) 10; 
        //damage
        data[2] = (float) 5.0f;
        //projectile Type
        data[3] = (string) "FireProjectile";
        //projectile speed
        data[4] = (float) 3.0f;
        //attack cooldown
        data[5] = (float) 3.0f;
        //base id
        data[6] = (int) 1000;
        return data;
    }

    public static object[] getSlowTowerData()
    {
        object[] data = new object[propertyCount];
        //name
        data[0] = (string) "SlowTower";
        //price
        data[1] = (int) 20; 
        //damage
        data[2] = (float) 7.0f;
        //projectile Type
        data[3] = (string) "SlowProjectile";
        //projectile speed
        data[4] = (float) 3.0f;
        //attack cooldown
        data[5] = (float) 3.0f;
        //base id
        data[6] = (int) 2000;
        return data;
    }

    public static object[] getLightTowerData()
    {
        object[] data = new object[propertyCount];
        //name
        data[0] = (string) "LightTower";
        //price
        data[1] = (int) 30; 
        //damage
        data[2] = (float) 10.0f;
        //projectile Type
        data[3] = (string) "LightProjectile";
        //projectile speed
        data[4] = (float) 3.0f;
        //attack cooldown
        data[5] = (float) 3.0f;
        //base id
        data[6] = (int) 3000;
        return data;
    }
}
