using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarDebugger : MonoBehaviour
{
    //fields
    //list of tiles for debugging, and arrow prefab for pointing to parents
    [SerializeField] private Tile startTile, goalTile;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private GameObject debugTilePrefab;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        clickTile();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            AStar.getPath(startTile.GetTilePosition());
        }
    }

    private void clickTile()
    {
        //mouse button 1 (right click)
        if (Input.GetMouseButtonDown(1))
        {
            //invisible ray cast from mouse position towards a tile
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            //if we hit something in the game world
            if (hit.collider != null)
            {
                //get the script from the tile
                Tile temp = hit.collider.GetComponent<Tile>();

                //not null means we hit a tile
                if (temp != null)
                {
                    //set the debugging state for the tiles clicked and change their color
                    if (startTile == null)
                    {
                        startTile = temp;
                        //startTile.setAStarDebugging(true);
                        //startTile.GetSpriteRenderer().color = new Color32(0, 132, 255, 255);
                        createDebugTile(startTile.GetWorldPosition(), new Color32(255, 135, 0, 255));

                    } else if (goalTile == null)
                    {
                        goalTile = temp;
                        //goalTile.setAStarDebugging(true);
                        //goalTile.GetSpriteRenderer().color = new Color32(255, 132, 0, 255);
                        createDebugTile(goalTile.GetWorldPosition(), new Color32(255, 0, 0, 255));
                    }
                }
            }
        }
    }

    //debugging function to show the path marked by AStar alg
    public void debugPath(HashSet<Node> openList, HashSet<Node> closedList)
    {
        foreach (Node node in openList)
        {
            //color the tiles yellow to help us visualize the open list path
            if (node.TileReference != startTile)
            {
                createDebugTile(node.TileReference.GetWorldPosition(), Color.yellow);
            }

            //points at the parent
            pointToParentNode(node, node.TileReference.GetWorldPosition());
        }



        foreach (Node node in closedList)
        {
            //color the tiles blue to help us visualize the closed list path
            if (node.TileReference != startTile && node.TileReference != goalTile)
            {
                createDebugTile(node.TileReference.GetWorldPosition(), Color.blue);
            }
        }
    }


    //debugging function so that neighbours point to their parent
    private void pointToParentNode(Node node, Vector2 position)
    {
        if (node.Parent != null)
        {
            //instantiate the arrow
            GameObject arrow = (GameObject)Instantiate(arrowPrefab, position, Quaternion.identity);

            //rotate the arrows based on their x and y position
            float x = node.Parent.GridPosition.X - node.GridPosition.X;
            float y = node.GridPosition.Y - node.Parent.GridPosition.Y;
            float rotationAngle = Mathf.Atan2(y, x) * 180 / Mathf.PI;

            arrow.transform.rotation = Quaternion.AngleAxis(rotationAngle, Vector3.forward);
        }
    }

    private void createDebugTile(Vector3 worldPos, Color32 color) 
    {
        //instantiate debug tile
        GameObject debugTile = (GameObject) Instantiate(debugTilePrefab, worldPos, Quaternion.identity);

        //set the color
        debugTile.GetComponent<SpriteRenderer>().color = color;
    }
}
