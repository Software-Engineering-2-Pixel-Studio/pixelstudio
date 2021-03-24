using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;

public class Tile : MonoBehaviour
{
    //fields
    //these 3 for the basic properties of a Tile
    private Point gridPos;
    private Vector3 worldPos;
    private Vector3 centerWorldPos;

    //these for indicate if a Tile is available for placing Tower
    private SpriteRenderer spriteRenderer;
    private bool isPlaced;
    private Color32 colorFullTile = new Color32(244, 183, 163, 255);
    private Color32 colorEmptyTile = new Color32(192, 255, 158, 255);

    //for debugging the aStar/enemy path algorithm
    private bool aStarDebugging;
    private bool isWalkable;


    // Start is called before the first frame update
    void Start()
    {
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void setTilePos(Point pointPosition)
    {
        this.gridPos = pointPosition;

    }


    private void setWorldPos(Vector3 worldPosition)
    {
        this.worldPos = worldPosition;
    }

    private void setCenterWorldPos(Vector3 worldPosition)
    {
        float tileSize = this.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        this.centerWorldPos = new Vector3(worldPosition.x + (tileSize / 2), worldPosition.y - (tileSize / 2), 0);
    }

    private void setSpriteRenderer(SpriteRenderer spriteRenderer)
    {
        this.spriteRenderer = spriteRenderer;
    }

    public void setAStarDebugging(bool debuggingState)
    {
        this.aStarDebugging = debuggingState;
    }

    public void setWalkable(bool walkableState)
    {
        this.isWalkable = walkableState;
    }

    //this function is called whenever the mouse is hover on this Tile
    private void OnMouseOver()
    {
        //make sure user have to pick a tower from buttons before place a tower on Map
        if (GameManager.Instance.GetPickedTowerButton() != null && !EventSystem.current.IsPointerOverGameObject())
        {
            if (!aStarDebugging)
            {
                //if this Tile has a placed Tower
                if (isPlaced)
                {
                    this.ColorTile(this.colorFullTile);
                }
                //else empty
                else
                {
                    this.ColorTile(this.colorEmptyTile);

                    //left-click at the tile to place the tower and leave out the last tile colum (for Tower Panel Overlay)
                    if (Input.GetMouseButtonDown(0) && gridPos.X < MapManager.Instance.GetXIndexSize() - 1)
                    {
                        PlaceTower();
                        MapManager.Instance.SetTileIsPlacedAt(this.gridPos.X, this.gridPos.Y);
                        //isPlaced = true;
                    }
                }
            }
        }
    }

    //this function start the process of placing tower on this Tile
    private void PlaceTower()
    {
        
        //get the tower prefab from GameManager
        GameObject towerPref = GameManager.Instance.GetPickedTowerButton().GetTowerPrefab();

        //Create the tower prefab on the scene
        //GameObject tower = Instantiate(towerPref);


        GameObject tower = PhotonNetwork.Instantiate(towerPref.name, this.GetCenterWorldPosition(), Quaternion.identity);

        //place at the correct position of the tile on the map
        tower.transform.position = this.GetCenterWorldPosition();

        //set the layer order based on Y gridposition
        tower.GetComponent<SpriteRenderer>().sortingOrder = this.gridPos.Y + 2;

        //set parent Tile for this tower
        tower.transform.SetParent(this.transform);

        //pay for this tower
        //GameManager.Instance.PayForPlacedTower();

        //deactive the Hover's spriterenderer
        Hover.Instance.Deactivate();

        //make the tile not walkable by enemy
        this.isWalkable = false;

        Debug.Log("Placed a tower!");
        
    }

    private void ColorTile(Color32 color)
    {
        this.spriteRenderer.color = color;
    }

    private void OnMouseExit()
    {
        if (!aStarDebugging)
        {
            ColorTile(Color.white);
        }
    }

    //this function Set up new Tile that created from MapManager
    public void SetUpTile(Point pointPosition, Vector3 worldPosition, Transform parent)
    {
        setTilePos(pointPosition);
        setWorldPos(worldPosition);
        setCenterWorldPos(worldPosition);
        MapManager.Instance.getTiles().Add(pointPosition, this);
        this.transform.position = worldPosition;
        this.transform.SetParent(parent);
        this.isPlaced = false;
        this.isWalkable = true;
    }

    public Point GetTilePosition()
    {
        return this.gridPos;
    }

    public Vector3 GetWorldPosition()
    {
        return this.worldPos;
    }

    public Vector3 GetCenterWorldPosition()
    {
        return this.centerWorldPos;
    }

    public SpriteRenderer GetSpriteRenderer()
    {
        return this.spriteRenderer;
    }

    public bool GetAStarDebuggingState()
    {
        return this.aStarDebugging;
    }
    
    public bool GetWalkableState()
    {
        return this.isWalkable;
    }

    public void SetIsPlaced(bool state)
    {
        this.isPlaced = state;
    }
}
