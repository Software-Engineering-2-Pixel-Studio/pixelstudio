using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public static Stack<Node> getPath(Point start, Point goal)
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

        //for storing the nodes we got from start to goal nodes
        Stack<Node> finalPath = new Stack<Node>();

        //reference the start node as the current node
        Node currentNode = nodes[start];

        //add the start node to the open list
        openList.Add(currentNode);

        while (openList.Count > 0) //until no path is available or until open list is empty
        {
            //get the neighbours from the current node and go through all neighbours
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    //get the surrounding position of neighbours
                    int xPosition = currentNode.GridPosition.X - x;
                    int yPosition = currentNode.GridPosition.Y - y;

                    Point neighbourPos = new Point(currentNode.GridPosition.X - x, currentNode.GridPosition.Y - y);

                    //check if tile is bounds
                    bool tileIsInBounds = MapManager.Instance.tileIsInBounds(neighbourPos);

                    //This is commented because if walkable state is checked here instead of inside the if statement,
                    //a bug happens (printed on the console as error) when the start goal is beside the edge of the screen
                    //check if the tile is walkable
                    //bool isWalkable = MapManager.Instance.getTiles()[neighbourPos].GetWalkableState();

                    //if tile is in bounds, the tile is walkable and the neighbour is not the current grid position
                    if (tileIsInBounds && MapManager.Instance.getTiles()[neighbourPos].GetWalkableState()
                        && !Point.arePointsEqual(neighbourPos, currentNode.GridPosition))
                    {
                        int gCost = 0; //set initial gcost to 0

                        //if it's left, top, bottom and right of the current node, gcost is 10
                        if (Math.Abs(x - y) == 1)
                        {
                            gCost = 10;
                        }
                        else
                        { //if it's diagonal from current node, gcost is set to 114 (a very high number)
                            if (!isConnectedDiagonally(currentNode, nodes[neighbourPos]))
                            {
                                //don't both and go to the next execution of the loop
                                continue;
                            }

                            //set to high number to disable diagonals in final pathing
                            gCost = 114;
                        }

                        //create nodes from neighbour position
                        Node neighbour = nodes[neighbourPos];

                        if (openList.Contains(neighbour))
                        {
                            //check if the g cost is lower if we assign current node as parent node
                            if (currentNode.gCost + gCost < neighbour.gCost)
                            {
                                //current node is the better parent
                                neighbour.calculateValues(currentNode, gCost, nodes[goal]);
                            }

                        }
                        else if (!closedList.Contains(neighbour)) //check all neighbours then ignore unwalkable nodes and nodes on the closed list
                        {
                            openList.Add(neighbour); //add to the open list the undiscovered neighbours

                            //set the current node as parent to new nodes on the open list
                            //also calculate F,G and H vals of new nodes
                            neighbour.calculateValues(currentNode, gCost, nodes[goal]);
                        }
                    }

                }
            }

            //removes the current node from open list and put it to closed list
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (openList.Count > 0)
            {
                //sorts the list by f value, take the node with the lowest f value and set it as the current node
                currentNode = openList.OrderBy(n => n.fCost).First();
            }

            if (currentNode == nodes[goal]) //until the goal is added to the closed list
            {
                while (!Point.arePointsEqual(currentNode.GridPosition, start))
                {
                    //add to the final path
                    finalPath.Push(currentNode);
                    currentNode = currentNode.Parent;
                }
                

                //we found a path so stop the loop
                break;
            }
        }

        return finalPath;

        //*****THIS IS ONLY FOR DEBUGGING NEEDS TO BE REMOVED LATER!*****
        //GameObject.Find("AStarDebugger").GetComponent<AStarDebugger>().debugPath(openList, closedList, finalPath);
    }

    private static bool isConnectedDiagonally(Node currentNode, Node neighbour)
    {
        bool result = true;

        //Point direction = neighbour.GridPosition - currentNode.GridPosition;
        Point direction = Point.calculateDifference(neighbour.GridPosition, currentNode.GridPosition);

        Point firstPoint = new Point(currentNode.GridPosition.X + direction.X, currentNode.GridPosition.Y);
        Point secondPoint = new Point(currentNode.GridPosition.X, currentNode.GridPosition.Y + direction.Y);

        //check if first point and second point are in bounds
        bool firstTileIsInBounds = MapManager.Instance.tileIsInBounds(firstPoint);
        bool secondTileIsInBounds = MapManager.Instance.tileIsInBounds(secondPoint);

        //if node is inbounds and is not walkable
        if ((firstTileIsInBounds && !MapManager.Instance.getTiles()[firstPoint].GetWalkableState()) 
            || (secondTileIsInBounds && !MapManager.Instance.getTiles()[secondPoint].GetWalkableState())) 
        {
            result = false;
        }

        return result;
    }
}
