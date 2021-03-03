using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AStar
{
    //dictionary of nodes in the game
    private static Dictionary<Point, Node> nodes;

    //Create nodes from tiles of the game
    private static void createNodes()
    {
        //instantiate the dictionary
        nodes = new Dictionary<Point, Node>();

        //go through each tile in the game and add them as nodes to the dictionary
        foreach (Tile tile in MapManager.Instance.getTiles().Values)
        {
            nodes.Add(tile.GetTilePosition(), new Node(tile));
        }
    }

    //create a path with A* algorithm
    public static void getPath(Point start)
    {
        //create nodes when we don't have the,
        if (nodes == null)
        {
            createNodes();
        }

        //create an open list for A* algorithm
        HashSet<Node> openList = new HashSet<Node>();

        //create a closed list for A* algorithm
        HashSet<Node> closedList = new HashSet<Node>();

        //reference the start node as the current node
        Node currentNode = nodes[start];

        //add the start node to the open list
        openList.Add(currentNode);

        //get the neighbours from the current node
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                //get the surrounding position of neighbours
                int xPosition = currentNode.GridPosition.X - x;
                int yPosition = currentNode.GridPosition.Y - y;
                Point neighbourPos = new Point(xPosition, yPosition);

                //check if tile is bounds
                bool tileIsInBounds = MapManager.Instance.tileIsInBounds(neighbourPos);

                //if tile is in bounds, the tile is walkable and the neighbour is not the current grid position
                if (tileIsInBounds && MapManager.Instance.getTiles()[neighbourPos].GetWalkableState() 
                    && !Point.arePointsEqual(neighbourPos, currentNode.GridPosition))
                {
                    //create nodes from neighbour position
                    Node neighbour = nodes[neighbourPos];

                    //Debugging for checking the neighbours visually
                    //neighbour.TileReference.GetSpriteRenderer().color = Color.cyan;

                    //add neighbours to the open list
                    if (!openList.Contains(neighbour))
                    {
                        openList.Add(neighbour);
                    }

                    //set the current node as the parents
                    neighbour.calculateValues(currentNode);
                }
               
            }
        }

        //removes the current node from open list and put it to closed list
        openList.Remove(currentNode);
        closedList.Add(currentNode);

        //*****THIS IS ONLY FOR DEBUGGING NEEDS TO BE REMOVED LATER!*****
        GameObject.Find("AStarDebugger").GetComponent<AStarDebugger>().debugPath(openList, closedList);
    }
}
