using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    //node's grid position
    public Point GridPosition { get; private set; }

    //the tile that this node belongs to
    public Tile TileReference { get; private set; }

    public Node Parent { get; private set; }

    public Node(Tile tileReference)
    {
        this.TileReference = tileReference;
        this.GridPosition = tileReference.GetTilePosition();
    }

    public void calculateValues(Node parent)
    {
        this.Parent = parent;
    }
}
