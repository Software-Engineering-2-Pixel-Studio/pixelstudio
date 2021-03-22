using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    //node's grid position
    public Point GridPosition { get; private set; }

    //the tile that this node belongs to
    public Tile TileReference { get; private set; }

    public Vector2 WorldPosition{ get; set; }

    public Node Parent { get; private set; }

    //for calculating the g,h, and f costs from start to goal tiles
    public int gCost { get; set; }
    public int hCost { get; set; }
    public int fCost { get; set; }

    public Node(Tile tileReference)
    {
        this.TileReference = tileReference;
        this.GridPosition = tileReference.GetTilePosition();
        this.WorldPosition = tileReference.GetWorldPosition();
    }

    //set the parent node and calculate the f,g and h values
    public void calculateValues(Node parent, int gCostVal, Node goal)
    {
        this.Parent = parent;
        this.gCost = parent.gCost + gCostVal;
        this.hCost = ((Math.Abs(GridPosition.X - goal.GridPosition.X)) + Math.Abs((goal.GridPosition.Y - GridPosition.Y))) * 10;
        this.fCost = gCost + hCost;
    }
}
