using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] tilePrefabs;

    public float TileSize{
        get{ 
            return tilePrefabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        CreateLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateLevel(){
        string[] mapData = LoadLevel();

        int mapsizeX = mapData[0].ToCharArray().Length;
        int mapsizeY = mapData.Length;

        Vector3 startPosition = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));

        for(int i = 0; i < mapsizeY; i++){
            char[] mapLine = mapData[i].ToCharArray();
            for(int j = 0; j < mapsizeX; j++){
               PlaceTile(mapLine[j].ToString(), j, i, startPosition);
            }
        }
    }

    private void PlaceTile(string tileType, int x, int y, Vector3 startPosition){
        int tileIndex = int.Parse(tileType);
        GameObject newTile = Instantiate(tilePrefabs[tileIndex]);
        newTile.transform.position = new Vector3(startPosition.x + (TileSize*x), startPosition.y - (TileSize*y), 0);
    }

    private string[] LoadLevel(){
        TextAsset map = Resources.Load("MapSlim") as TextAsset;

        string tempMap = map.text.Replace(Environment.NewLine, string.Empty);

        return tempMap.Split('-');
    }
}