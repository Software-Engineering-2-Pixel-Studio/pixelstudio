using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayerManager : MonoBehaviour
{
    //fields
    [SerializeField] private GameObject player;
    [SerializeField] private float minX, minY, maxX, maxY;

    // Start is called before the first frame update
    private void Start()
    {
        minX = -8.4f;
        maxX = 8.4f;
        minY = -4.4f;
        maxY = 4.4f;
        Vector2 randomPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
        PhotonNetwork.Instantiate(player.name, randomPosition, Quaternion.identity);
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
