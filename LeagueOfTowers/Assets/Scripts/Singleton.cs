using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this singleton class is for those script that appears only one in the game.
public abstract class Singleton<T> : MonoBehaviour where T: MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
            }

            return instance;
        }
    }

    //above code are the same with this
    /*public static T getInstance()
    {
        if(instance == null)
        {
            instance = FindObjectOfType<T>();
        }
        return instance;
    }*/
}
