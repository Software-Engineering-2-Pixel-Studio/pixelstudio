using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public enum PlayerClass: int
{
    MAGE = 0,
    DEFENDER = 1
    
}

public class SpawnPlayerManager : Singleton<SpawnPlayerManager>
{
    //fields
    [SerializeField] private GameObject[] playerClasses = new GameObject[2];      //player's prefabs
    [SerializeField] private GameObject pickClassMenu;
    //private PlayerClass pickedClass;

    // Start is called before the first frame update
    private void Start()
    {
        this.pickClassMenu.SetActive(true);
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    public void MageClassPicked()
    {
        Debug.Log("Mage class created");
        //this.pickedClass = PlayerClass.MAGE;
        //spawn player at center of the world
        Vector2 position = new Vector2(0,0);
        PhotonNetwork.Instantiate(playerClasses[(int)PlayerClass.MAGE].name, position, Quaternion.identity);
        this.pickClassMenu.SetActive(false);
    }

    public void DefenderClassPicked()
    {
        Debug.Log("Defender class created");
        //this.pickedClass = PlayerClass.DEFENDER;
        Vector2 position = new Vector2(0,0);
        PhotonNetwork.Instantiate(playerClasses[(int)PlayerClass.DEFENDER].name, position, Quaternion.identity);
        this.pickClassMenu.SetActive(false);
    }
}
