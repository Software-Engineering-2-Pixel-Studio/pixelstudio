using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayerManager : Singleton<SpawnPlayerManager>
{
    //fields
    [SerializeField] private GameObject player;      //player's prefab

    // Start is called before the first frame update
    private void Start()
    {
        //spawn player at center of the world
        Vector2 position = new Vector2(0,0);
        PhotonNetwork.Instantiate(player.name, position, Quaternion.identity);
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
