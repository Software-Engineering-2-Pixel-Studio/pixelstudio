using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;

public class Tile : MonoBehaviour
{
    //fields
    //private int tileID;

    [SerializeField]
    private string tileName;

    private int tileID;
    //these 3 for the basic properties of a Tile
    private Point gridPos;
    private Vector3 worldPos;
    private Vector3 centerWorldPos;

    //these for indicate if a Tile is available for placing Tower
    private SpriteRenderer spriteRenderer;
    [SerializeField] private bool isPlaced;
    private Color32 colorFullTile = new Color32(244, 183, 163, 255);
    private Color32 colorEmptyTile = new Color32(192, 255, 158, 255);

    //for debugging the aStar/enemy path algorithm
    private bool aStarDebugging;
    private bool isWalkable;

    //tower
    private TowerScript myTower;

    // Start is called before the first frame update
    private void Start()
    {
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    /*
        Method to set Tile position on the Grid
    */
    private void setTilePos(Point pointPosition)
    {
        this.gridPos = pointPosition;

    }

    /*
        Method to set Tile position on the world (top-left corner)
    */
    private void setWorldPos(Vector3 worldPosition)
    {
        this.worldPos = worldPosition;
    }

    /*
        Method to set Tile center position on the world
    */
    private void setCenterWorldPos(Vector3 worldPosition)
    {
        float tileSize = this.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        this.centerWorldPos = new Vector3(worldPosition.x + (tileSize / 2), worldPosition.y - (tileSize / 2), 0);
    }

    /*
        Method to set the image of this Tile
    */
    private void setSpriteRenderer(SpriteRenderer spriteRenderer)
    {
        this.spriteRenderer = spriteRenderer;
    }

    /*
        Method to set the debug state of this Tile
    */
    public void setAStarDebugging(bool debuggingState)
    {
        this.aStarDebugging = debuggingState;
    }

    /*
        Method to set the walkable state of this Tile
    */
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
        else if (!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.GetPickedTowerButton() == null && Input.GetMouseButtonDown(0))
        {
            //Debug.Log("clicked");
            if (myTower != null)
            {
                //Debug.Log("myTower is not null");
                SelectTowerManager.Instance.SelectTower(myTower);
            } 
            else
            {
                //Debug.Log("myTower is null");
                SelectTowerManager.Instance.DeselectTower();
            }
        }
    }

    //this function start the process of placing tower on this Tile
    private void PlaceTower()
    {
        
        //get the tower prefab from GameManager
        TowerButton pickedButton = GameManager.Instance.GetPickedTowerButton();
        GameObject towerPref = GameManager.Instance.GetPickedTowerButton().GetTowerPrefab();

        //Create the tower prefab on the scene
        //GameObject tower = Instantiate(towerPref);

        //object[] basicData = new object[8];
        List<object> lo = new List<object>();
        string towerType = pickedButton.GetTowerName();
        if(towerType == "FireTower")
        {
            //basicData = TowerData.getFireTowerData(this.tileID);
            lo = TowerData.GetFireTowerData(this.tileID);
        }
        else if(towerType == "SlowTower")
        {
            //basicData = TowerData.getSlowTowerData(this.tileID);
            lo = TowerData.GetSlowTowerData(this.tileID);
        }
        else if(towerType == "LightTower")
        {
            //basicData = TowerData.getLightTowerData(this.tileID);
            lo = TowerData.GetLightTowerData(this.tileID);
        }
        else if(towerType == "BaseTower")
        {
            lo = TowerData.GetBaseTowerData(this.tileID);
        }

        //object[] data = new object[8];
        // if(basicData != null){
        //     data[0] = basicData[0]; //name
        //     data[1] = basicData[1]; //price
        //     data[2] = basicData[2]; //damage
        //     data[3] = basicData[3]; //projectileType
        //     data[4] = basicData[4]; //projectileSpeed
        //     data[5] = basicData[5]; //cooldown
        //     data[6] = basicData[6]; //baseID
        //     data[7] = (int) this.tileID;    //tileID where this tower is placed
        // }
        //object[] data;
        //GameObject tower;
        // if(towerType == "FireTower")
        // {
        //     data = TowerData.getFireTowerData(this.tileID);
            
        // }
        // else if(towerType == "SlowTower")
        // {
        //     data = TowerData.getSlowTowerData(this.tileID);
        // }
        // else if(towerType == "LightTower")
        // {
        //     data = TowerData.getLightTowerData(this.tileID);
        // }
        GameObject tower = PhotonNetwork.Instantiate(towerPref.name, this.GetCenterWorldPosition(), Quaternion.identity,0, lo.ToArray() );
        // if(towerType == "FireTower")
        // {
        //     this.myTower = tower.transform.GetComponent<FireTowerScript>();
        // }
        // else
        // {
        //     Debug.Log("WTF this is called");
        //     this.myTower = tower.transform.GetComponent<TowerScript>();
        //     Debug.Log(myTower);
        // }
        this.myTower = tower.transform.GetComponent<TowerScript>();
        this.myTower.GetComponent<SpriteRenderer>().sortingOrder = this.gridPos.Y + 2;
        MapManager.Instance.SetTileIsPlacedAt2(this.tileID, true);
        GameManager.Instance.PayForPlacedTower();
        

        //deactive the Hover's spriterenderer
        Hover.Instance.Deactivate();

        //make the tile not walkable by enemy
        this.isWalkable = false;

        //Debug.Log("Placed a tower!");
        
    }

    /*
        Method to set color of this Tile
    */
    private void ColorTile(Color32 color)
    {
        this.spriteRenderer.color = color;
    }

    /*
        Method to checking state of mouse event when exit this Tile
    */
    private void OnMouseExit()
    {
        if (!aStarDebugging)
        {
            ColorTile(Color.white);
        }
    }

    //this function Set up new Tile that created from MapManager
    public void SetUpTile(int tileID, Point pointPosition, Vector3 worldPosition, Transform parent, int tileIndex)
    {
        this.tileID = tileID;
        this.tileName = "Tile" + this.tileID;
        this.gameObject.name = this.tileName;
        setTilePos(pointPosition);
        setWorldPos(worldPosition);
        setCenterWorldPos(worldPosition);
        MapManager.Instance.getTiles().Add(pointPosition, this);
        this.transform.position = worldPosition;
        this.transform.SetParent(parent);
        this.isPlaced = false;
        if(tileIndex == 0){
            this.isWalkable = false;
        }
        else{
            this.isWalkable = true;
            this.isPlaced = true;
        }

        
    }

    /*
        Method to get Tile position on the Grid
    */
    public Point GetTilePosition()
    {
        return this.gridPos;
    }

    /*
        Method to get Tile position on the World (top-left corner)
    */
    public Vector3 GetWorldPosition()
    {
        return this.worldPos;
    }

    /*
        Method to get Tile center position on the World 
    */
    public Vector3 GetCenterWorldPosition()
    {
        return this.centerWorldPos;
    }

    /*
        Method to get the Tile image
    */
    public SpriteRenderer GetSpriteRenderer()
    {
        return this.spriteRenderer;
    }

    /*
        Method to get the debug state of this Tile
    */
    public bool GetAStarDebuggingState()
    {
        return this.aStarDebugging;
    }
    
    /*
        Method to get the walkable state of this Tile
    */
    public bool GetWalkableState()
    {
        return this.isWalkable;
    }

    /*
        Method to set the IsPlaced State of this Tile
    */
    public void SetIsPlaced(bool state)
    {
        this.isPlaced = state;
    }

    public override string ToString(){
        return this.tileName;
    }

    public void SetMyTower(TowerScript tower){
        this.myTower = tower;
    }
}
