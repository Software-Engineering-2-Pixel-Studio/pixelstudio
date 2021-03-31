using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTower : Tower
{
    private void Start()
    {
        setElementType(Element.SLOW);
    }
}
