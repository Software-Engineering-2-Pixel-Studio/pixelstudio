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
    //quickly shoot, low damage, none element, cheap
    public static List<object> GetBaseTowerData(int tileID)
    {
        List<object> data = new List<object>();
        //name
        data.Add((string) "BaseTower");
        //price
        data.Add((int) 10);
        //damage
        data.Add((float) 5.0f);
        //typeproj
        data.Add ((string) "BaseProjectile");
        //speed
        data.Add((float) 4.0f);
        //cooldown
        data.Add((float) 2.0f);
        //base id
        data.Add((int) 1000);
        //tileID
        data.Add((int) tileID);
        //Element
        data.Add((byte) Element.NONE);
        //debuffchance
        data.Add((float) 50.0f);
        //duration
        data.Add((float) 0.0f);
        
        return data;
    }

    public static List<object> GetBaseTowerTechUpData()
    {
        List<object> data = new List<object>();
        //projectileSpeed
        data.Add((float) 1.0f);
        //acttackCoolDown
        data.Add((float) -1.0f);
        
        return data;
    }

    public static List<object> GetBaseTowerUp1Data()
    {
        List<object> data = new List<object>();
        //cost
        data.Add((int) 10);
        //plus damage
        data.Add((float) 5.0f);

        return data;
    }

    public static List<object> GetBaseTowerUp2Data()
    {
        List<object> data = new List<object>();
        //cost
        data.Add((int) 20);
        //plus damage
        data.Add((float) 10.0f);

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
        //debuffchance
        data.Add((float) 50.0f);
        //duration
        data.Add((float) 4.0f);
        //tickDamage
        data.Add((float) 2.5f);
        //tickTime
        data.Add((float) 1.0f);
        
        return data;
    }

    public static List<object> GetFireTowerTechUpData()
    {
        List<object> data = new List<object>();
        //projectileSpeed
        data.Add((float) 1.0f);
        //acttackCoolDown
        data.Add((float) -1.0f);
        return data;
    }

    public static List<object> GetFireTowerUp1Data()
    {
        List<object> data = new List<object>();
        //cost
        data.Add((int) 20);
        //plus damage
        data.Add((float) 5.0f);
        //plus duration
        data.Add((float) 1.0f);
        //plus chance
        data.Add((float) 10.0f);
        //plus tick damage
        data.Add((float) 1.5f);
        //plus tick time
        data.Add((float) 0.1);

        return data;
    }

    public static List<object> GetFireTowerUp2Data()
    {
        List<object> data = new List<object>();
        //cost
        data.Add((int) 40);
        //plus damage
        data.Add((float) 10.0f);
        //plus duration
        data.Add((float) 1.0f);
        //plus chance
        data.Add((float) 10.0f);
        //plus tick damage
        data.Add((float) 2.0f);
        //plus tick time
        data.Add((float) 0.1);

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
        //debuffchance
        data.Add((float) 50.0f);
        //duration
        data.Add((float) 2.0f);
        //slowFactor
        data.Add((float) 1.0);
        
        return data;
    }

    public static List<object> GetSlowTowerTechUpData()
    {
        List<object> data = new List<object>();
        //projectileSpeed
        data.Add((float) 1.0f);
        //acttackCoolDown
        data.Add((float) -1.0f);
        return data;
    }

    public static List<object> GetSlowTowerUp1Data()
    {
        List<object> data = new List<object>();
        //cost
        data.Add((int) 20);
        //plus damage
        data.Add((float) 5.0f);
        //plus durarion
        data.Add((float) 1.0f);
        //plus chance
        data.Add((float) 10.0f);
        //plus slow factor
        data.Add((float) 0.25f);

        return data;
    }

    public static List<object> GetSlowTowerUp2Data()
    {
        List<object> data = new List<object>();
        //cost
        data.Add((int) 40);
        //plus damage
        data.Add((float) 5.0f);
        //plus durarion
        data.Add((float) 1.0f);
        //plus chance
        data.Add((float) 10.0f);
        //plus slow factor
        data.Add((float) 0.25f);

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
        //debuffchance
        data.Add((float) 50.0f);
        //duration
        data.Add((float) 1.0f);
        
        return data;
    }

    public static List<object> GetLightTowerTechUpData()
    {
        List<object> data = new List<object>();
        //projectileSpeed
        data.Add((float) 1.0f);
        //acttackCoolDown
        data.Add((float) -1.0f);
        return data;
    }

    public static List<object> GetLightTowerUp1Data()
    {
        List<object> data = new List<object>();
        //cost
        data.Add((int) 30);
        //plus damage
        data.Add((float) 10.0f);
        //plus duration
        data.Add((float) 0.5f);
        //plus chance
        data.Add((float) 10.0f);

        return data;
    }

    public static List<object> GetLightTowerUp2Data()
    {
        List<object> data = new List<object>();
        //cost
        data.Add((int) 60);
        //plus damage
        data.Add((float) 10.0f);
        //plus duration
        data.Add((float) 0.5f);
        //plus chance
        data.Add((float) 10.0f);

        return data;
    }
}
