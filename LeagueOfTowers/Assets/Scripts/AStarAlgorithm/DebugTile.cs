using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugTile : MonoBehaviour
{
    [SerializeField] private Text fText;
    [SerializeField] private Text gText;
    [SerializeField] private Text hText;

    public Text F { 
        get
        {
            fText.gameObject.SetActive(true);
            return fText;
        }
        set
        {
            this.fText = value;
        }
    }

    public Text G
    {
        get
        {
            gText.gameObject.SetActive(true);
            return gText;
        }
        set
        {
            this.gText = value;
        }
    }

    public Text H
    {
        get
        {
            hText.gameObject.SetActive(true);
            return hText;
        }
        set
        {
            this.hText = value;
        }
    }

}
