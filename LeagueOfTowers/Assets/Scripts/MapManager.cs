using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;
public class MapManager : Singleton<MapManager>
{
    //fields
    //list of tile prefabs
    [SerializeField] private GameObject[] tilePrefabs;  //list of tile's prefabs
    private PhotonView view;        //PhotonView object to synchronize

    //mapIndexSize;
    private int xIndexSize;         //number of Tiles in horizontal
    private int yIndexSize;         //number of Tiles in vertical

    //map bounds
    private float minMapX;
    private float maxMapX;
    private float minMapY;
    private float maxMapY;

    private float[] mapBounds = new float[4];   //minX,maxX,minY,maxY

    //a dictionary of tiles, where each tile mapped on a gridposition of the map
    private Dictionary<Point, Tile> Tiles;      //key = GridPoint, value = Tile
    private Dictionary<int, Tile> tile2;        //key = tileID, value = Tile

    //start and end pathing
    private Point spawnPos;

    public Point SpawnPos{
        get{
            return spawnPos;
        }
    }

    [SerializeField] private GameObject spawnPrefab;        //the spawn portal prefab

    public Portal SpawnPrefab{ get; set;}          //get, set property for spawn portal object

    private Point basePos;  //base position base on grid of the map
    [SerializeField] private GameObject basePrefab;

    //a parent object to put all the Tile under
    [SerializeField] private Transform ground;

    //size of map
    private Point mapSize;

    // path for monsters to use
    private Stack<Node> path;

    // the stac that holds the path of the monster
    public Stack<Node> Path
    {
        get{
            if(path == null){
                GeneratePath();
            }
            return new Stack<Node>(path);
        }
    }

    //methods

    //function above is the same with this (a C# feature)
    public float TileSize
    {
        get { return tilePrefabs[1].GetComponent<SpriteRenderer>().sprite.bounds.size.x; }
    }

    void Awake()
    {

    }

    // Start is called before the first frame update
    private void Start()
    {
        view = this.GetComponent<PhotonView>();
        CreateLevel();
    }

    // Update is called once per frame
    private void Update()
    {

    }
    
    /*
     * CreateLevel: generate the map (ground) base on the camera view
     */
    private void CreateLevel()
    {
        Tiles = new Dictionary<Point, Tile>();
        tile2 = new Dictionary<int, Tile>();
        string[] mapData = ReadLevelText();
        

        //set the mapsize
        mapSize = new Point(mapData[0].ToCharArray().Length, mapData.Length);

        //this line set the origin point to the topleft screen.
        Vector3 worldStart = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0));

        xIndexSize = mapData[0].ToCharArray().Length;
        yIndexSize = mapData.Length;

        //generate map
        //string tileID;
        int tileCount = 0;
        for (int y = 0; y < yIndexSize; y++)       //y-coords
        {
            char[] newTiles = mapData[y].ToCharArray();
            for (int x = 0; x < xIndexSize; x++) //x-coords
            {      
                PlaceTile(tileCount, newTiles[x].ToString(), x, y, worldStart);
                tileCount++;
                
            }
        }

        //place spawn
        SetUpSpawn();

        //place base
        SetUpBase();

        setMapBounds(worldStart);

        
    }//end of CreateLevel


    /*
     * PlaceTile: create a tile prefab at a specific location on scene.
     */
    private void PlaceTile(int count, string tileType, int x, int y, Vector3 worldStart)
    {
        int tileIndex = int.Parse(tileType);
        //string tileID = y.ToString() + x.ToString();
        int tileID = count;

        int randomIndex;
        if (tileIndex == 0) //for the grass
        {
            randomIndex = UnityEngine.Random.Range(0, 4); //0-3
        }
        else //for the gray walkable tiles
        {
            randomIndex = UnityEngine.Random.Range(4, 8); //4-7
        }


        //Position on the grid where we place the tile
        Point gridPos = new Point(x, y);

        //The actual position on the scene where we place the tile
        Vector3 worldPos = new Vector3(worldStart.x + TileSize * x, worldStart.y - TileSize * y, 0);

        GameObject nTile = Instantiate(tilePrefabs[randomIndex]);
        
        Tile nTileScript = nTile.GetComponent<Tile>();

        //setup and place this new tile on world scene
        nTileScript.SetUpTile(tileID, gridPos, worldPos, ground, tileIndex);
        
        tile2.Add(tileID, nTileScript);
        
    }

    // gets the Maps representation from an external file and reads it
    private string[] ReadLevelText()
    {
        TextAsset bindData = (TextAsset)Resources.Load("Map");

        string data = bindData.text.Replace(Environment.NewLine, string.Empty);

        return data.Split('-');
    }

    /*

    */
    private void setMapBounds(Vector3 worldStart)
    {
        this.minMapX = worldStart.x;
        this.maxMapX = worldStart.x + TileSize * (xIndexSize);
        this.minMapY = worldStart.y - TileSize * (yIndexSize);
        this.maxMapY = worldStart.y;
    }

    private float[] getMapBounds()
    {
        
        this.mapBounds[0] = this.minMapX;
        this.mapBounds[1] = this.maxMapX;
        this.mapBounds[2] = this.minMapY;
        this.mapBounds[3] = this.maxMapY;

        return mapBounds ;
    }

    public GameObject getTile(int id){
        return this.ground.GetChild(id).gameObject;
    }

    public Tile GetSpawnTile()
    {
        return this.Tiles[spawnPos];
    }

    public Tile GetBaseTile()
    {
        return this.Tiles[basePos];
    }

    /*
        Method to create the spawn portal on the map
    */
    private void SetUpSpawn()
    {
        spawnPos = new Point(0, 1);

        //create a spawn prefab on the scene.
        GameObject theSpawn = Instantiate(spawnPrefab);

        //place spawn at gridPos = (0,1) or start of the path
        theSpawn.transform.position = Tiles[spawnPos].GetCenterWorldPosition();
        

        SpawnPrefab = theSpawn.GetComponent<Portal>();  //get script to  the reference
        SpawnPrefab.name = "SpawnPrefab";               // rename it 
        //^^^ needed to use later when spawn enemies
    }

    /*
        Method to create the base on the map
    */
    private void SetUpBase()
    {
        basePos = new Point(15, 5);

        //create a base prefab on the scene
        GameObject theBase = Instantiate(basePrefab);

        //place the base at grid(15,5) or end of the path
        theBase.transform.position = Tiles[basePos].GetCenterWorldPosition();

    }

    /*
        Method to get dictionary of Tiles
    */
    public Dictionary<Point, Tile> getTiles()
    {
        return this.Tiles;
    }

    public Dictionary<int, Tile> getTile2(){
        return this.tile2;
    }

    //return number of tiles in horizontal of the map
    public int GetXIndexSize()
    {
        return this.xIndexSize;
    }

    //return number of tiles in vertical of the map
    public int GetYIndexSize()
    {
        return this.yIndexSize;
    }

    //test if a tile is inbound
    public bool tileIsInBounds(Point position)
    {
        //check if it's not past the left and right boundary of the map
        bool isGreaterThanLeftMapBoundary = position.X >= 0 && position.Y >= 0;
        bool isLessThanRightMapBoundary = position.X < mapSize.X && position.Y < mapSize.Y;

        return isGreaterThanLeftMapBoundary && isLessThanRightMapBoundary;
    }

    /*
        Method to generate path for monster from spawn portal to base 
    */
    public void GeneratePath()
    {
        path = AStar.getPath(basePos,spawnPos); 
    }

    

    /*
        PunRPC
        Sync a specific tile to be full
    */
    [PunRPC]
    private void setTileIsPlacedAtRPC(int gridPointX, int gridPointY)
    {
        Point gP = new Point(gridPointX, gridPointY);
        this.Tiles[gP].SetIsPlaced(true);
        //Debug.Log("RPC called for setTileIsPlacedRPC to true");
    }

    /*
        Sync a specific tile to be full and send signal to other players
    */
    public void SetTileIsPlacedAt(int gridPointX, int gridPointY)
    {
        this.view.RPC("setTileIsPlacedAtRPC", RpcTarget.All, gridPointX, gridPointY);
    }

    /*
        Method to set the Tile is empty so we can place tower on it
    */
    [PunRPC]
    private void setTileIsEmptyAtRPC(int gridPointX, int gridPointY)
    {
        Point gP = new Point(gridPointX, gridPointY);
        this.Tiles[gP].SetIsPlaced(false);
        //Debug.Log("RPC called for setTileIsPlacedRPC to true");
    }

    /*
        Method to set the Tile is empty so we can place tower on it, also send the signal
        to every player that this variable has been changed
    */
    public void SetTileIsEmptyAt(int gridPointX, int gridPointY)
    {
        this.view.RPC("setTileIsEmptyAtRPC", RpcTarget.All, gridPointX, gridPointY);
    }

    /*
        Pair of methods to set the tile state over network
    */
    [PunRPC]
    private void setTileIsPlacedAtRPC2(int tileID, bool state)
    {
        this.tile2[tileID].SetIsPlaced(state);
        if(state == false)  //is not placed
        {
            this.tile2[tileID].SetMyTower(null);
        }
    }

    public void SetTileIsPlacedAt2(int tileID, bool state)
    {
        this.view.RPC("setTileIsPlacedAtRPC2", RpcTarget.All, tileID, state);
    }

}
