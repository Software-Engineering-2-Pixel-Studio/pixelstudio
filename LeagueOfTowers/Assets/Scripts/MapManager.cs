using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapManager : Singleton<MapManager>
{
    //fields
    //list of tile prefabs
    [SerializeField] private GameObject[] tilePrefabs;
    [SerializeField] private CameraMovement camMove;
    [SerializeField] private CameraFollow camFollow;

    //mapIndexSize;
    private int xIndexSize;
    private int yIndexSize;

    //map bounds
    private float minMapX;
    private float maxMapX;
    private float minMapY;
    private float maxMapY;

    private float[] mapBounds = new float[4];   //minX,maxX,minY,maxY

    //a dictionary of tiles, where each tile mapped on a gridposition of the map
    private Dictionary<Point, Tile> Tiles;

    //start and end pathing
    private Point spawnPos;

    public Point SpawnPos{
        get{
            return spawnPos;
        }
    }

    [SerializeField] private GameObject spawnPrefab;

    public Portal SpawnPrefab{ get; set;}

    private Point basePos;
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
            return new Stack<Node>(new Stack<Node>(path));
        }
    }

    //methods
    /*public float getTileSize()
    {
        //extract width (x) value of a sprite 
        return tile.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
    }*/

    //function above is the same with this (a C# feature)
    public float TileSize
    {
        get { return tilePrefabs[1].GetComponent<SpriteRenderer>().sprite.bounds.size.x; }
    }

    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        //set up prefab Tile_Grass_0 in Prefabs folder to variable tile at runtime
        /*tile = (GameObject) Resources.Load("Prefabs/Tile_Grass_0", typeof(GameObject));*/

        CreateLevel();
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    /*
     * CreateLevel: generate the map (ground) base on the camera view
     */
    private void CreateLevel()
    {
        Tiles = new Dictionary<Point, Tile>();
        string[] mapData = ReadLevelText();

        //set the mapsize
        mapSize = new Point(mapData[0].ToCharArray().Length, mapData.Length);

        //this line set the origin point to the topleft screen.
        Vector3 worldStart = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0));

        /*int mapXSize = mapData[0].ToCharArray().Length;
        int mapYSize = mapData.Length;*/

        xIndexSize = mapData[0].ToCharArray().Length;
        yIndexSize = mapData.Length;

        //generate map
        for (int y = 0; y < yIndexSize; y++)       //y-coords
        {
            char[] newTiles = mapData[y].ToCharArray();
            for (int x = 0; x < xIndexSize; x++) //x-coords
            {
                PlaceTile(newTiles[x].ToString(), x, y, worldStart);

            }
        }

        //place spawn
        SetUpSpawn();

        //place base
        SetUpBase();

        setMapBounds(worldStart);
        camMove.SetBounders(this.getMapBounds());
        camFollow.SetBounders(this.getMapBounds());

        
    }//end of CreateLevel


    /*
     * PlaceTile: create a tile prefab at a specific location on scene.
     */
    private void PlaceTile(string tileType, int x, int y, Vector3 worldStart)
    {
        int tileIndex = int.Parse(tileType);

        int randomIndex;
        if (tileIndex == 0)
        {
            randomIndex = UnityEngine.Random.Range(0, 4); //0-3
        }
        else
        {
            randomIndex = UnityEngine.Random.Range(4, 8); //4-7
        }


        //Position on the grid where we place the tile
        Point gridPos = new Point(x, y);

        //The actual position on the scene where we place the tile
        Vector3 worldPos = new Vector3(worldStart.x + TileSize * x, worldStart.y - TileSize * y, 0);

        //Instantiate(tilePrefabs[randomIndex]) = create new tile from tile prefabs
        //Tile newTile = .....GetComponent<Tile>() mean extract the Tile script from this new tile
        Tile newTile = Instantiate(tilePrefabs[randomIndex]).GetComponent<Tile>();

        //setup and place this new tile on world scene
        newTile.SetUpTile(gridPos, worldPos, ground);

        //also, add this new tile along with its gridPos to the dictionary
        //this can be done from Tile script with Singleton help
        //Tiles.Add(gridPos, newTile);
        
    }

    // gets the Maps representation from an external file and reads it
    private string[] ReadLevelText()
    {
        TextAsset bindData = (TextAsset)Resources.Load("Map");

        string data = bindData.text.Replace(Environment.NewLine, string.Empty);

        return data.Split('-');
    }

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

    private void SetUpBase()
    {
        basePos = new Point(16, 5);

        //create a base prefab on the scene
        GameObject theBase = Instantiate(basePrefab);

        //place the base at grid(16,5) or end of the path
        theBase.transform.position = Tiles[basePos].GetCenterWorldPosition();
    }

    public Dictionary<Point, Tile> getTiles()
    {
        return this.Tiles;
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

    // generate a path for the enemies using the AStar algorithm
    public void GeneratePath(){
        path = AStar.getPath(spawnPos, basePos); 
    }
}
