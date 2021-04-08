using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Element : byte
{
    NONE = 0,
    FIRE,
    LIGHT,
    SLOW
    
}

public class TowerData : Singleton<TowerData>
{
    // private const int propertyCount = 7;
    // public static object[] getFireTowerData(int tileID)
    // {
    //     object[] data = new object[8];
    //     //name
    //     data[0] = (string) "FireTower";
    //     //price
    //     data[1] = (int) 10; 
    //     //damage
    //     data[2] = (float) 5.0f;
    //     //projectile Type
    //     data[3] = (string) "FireProjectile";
    //     //projectile speed
    //     data[4] = (float) 3.0f;
    //     //attack cooldown
    //     data[5] = (float) 3.0f;
    //     //base id
    //     data[6] = (int) 1000;
    //     //tileID
    //     data[7] = (int) tileID;
    //     return data;
    // }

    // public static object[] getSlowTowerData(int tileID)
    // {
    //     object[] data = new object[8];
    //     //name
    //     data[0] = (string) "SlowTower";
    //     //price
    //     data[1] = (int) 20; 
    //     //damage
    //     data[2] = (float) 7.0f;
    //     //projectile Type
    //     data[3] = (string) "SlowProjectile";
    //     //projectile speed
    //     data[4] = (float) 3.0f;
    //     //attack cooldown
    //     data[5] = (float) 3.0f;
    //     //base id
    //     data[6] = (int) 2000;
    //     //tileId
    //     data[7] = (int) tileID;
    //     return data;
    // }

    // public static object[] getLightTowerData(int tileID)
    // {
    //     object[] data = new object[8];
    //     //name
    //     data[0] = (string) "LightTower";
    //     //price
    //     data[1] = (int) 30; 
    //     //damage
    //     data[2] = (float) 10.0f;
    //     //projectile Type
    //     data[3] = (string) "LightProjectile";
    //     //projectile speed
    //     data[4] = (float) 3.0f;
    //     //attack cooldown
    //     data[5] = (float) 3.0f;
    //     //base id
    //     data[6] = (int) 3000;
    //     //tileID
    //     data[7] = (int) tileID;
    //     return data;
    // }

    //quickly shoot, low damage, none element, cheap
    public static List<object> GetBasicTowerData(int tileID)
    {
        List<object> data = new List<object>();
        //name
        data.Add((string) "BasicTower");
        //price
        data.Add((int) 10);
        //damage
        data.Add((float) 5.0f);
        //typeproj
        data.Add ((string) "BaseProjectile");
        //speed
        data.Add((float) 4.0f);
        //cooldown
        data.Add((float) 1.0f);
        //base id
        data.Add((int) 1000);
        //tileID
        data.Add((int) tileID);
        //Element
        data.Add((byte) Element.NONE);
        
        
        return data;
    }

    //average shoot, fire element, tick damage, average cost, high damage
    public static List<object> GetFireTowerData(int tileID)
    {
        List<object> data = new List<object>();
        //name
        data.Add((string) "FireTower");
        //price
        data.Add((int) 20);
        //damage
        data.Add((float) 7.5f);
        //typeproj
        data.Add ((string) "FireProjectile");
        //speed
        data.Add((float) 3.0f);
        //cooldown
        data.Add((float) 3.0f);
        //base id
        data.Add((int) 2000);
        //tileID
        data.Add((int) tileID);
        //Element
        data.Add((byte) Element.FIRE);
        //tickDamage
        data.Add((float) 2.5f);
        //tickTime
        data.Add((float) 1.0f);
        
        
        return data;
    }

    //average shoot, slow element, slow down enemy, average cost, low damage
    public static List<object> GetSlowTowerData(int tileID)
    {
        List<object> data = new List<object>();
        //name
        data.Add((string) "SlowTower");
        //price
        data.Add((int) 20);
        //damage
        data.Add((float) 5.0f);
        //typeproj
        data.Add ((string) "SlowProjectile");
        //speed
        data.Add((float) 3.0f);
        //cooldown
        data.Add((float) 3.0f);
        //base id
        data.Add((int) 3000);
        //tileID
        data.Add((int) tileID);
        //Element
        data.Add((byte) Element.SLOW);
        //slowFactor
        data.Add((float) 1.0);
        
        
        return data;
    }

    //slow shoot, light element, high cost, very high damage, make element type stop
    public static List<object> GetLightTowerData(int tileID)
    {
        List<object> data = new List<object>();
        //name
        data.Add((string) "LightTower");
        //price
        data.Add((int) 30);
        //damage
        data.Add((float) 10.0f);
        //typeproj
        data.Add ((string) "LightProjectile");
        //speed
        data.Add((float) 3.0f);
        //cooldown
        data.Add((float) 5.0f);
        //base id
        data.Add((int) 4000);
        //tileID
        data.Add((int) tileID);
        //Element
        data.Add((byte) Element.LIGHT);
        
        return data;
    }
}
