using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTower : Tower
{
    private void Start()
    {
        setElementType(Element.LIGHT);
    }
}
