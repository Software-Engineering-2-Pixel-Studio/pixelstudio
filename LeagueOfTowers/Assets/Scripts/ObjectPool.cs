using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ObjectPool : MonoBehaviour
{
    // variables to control the pool of the generated game objects
    [SerializeField]
    private GameObject[] objectPrefabs;

    private List<GameObject> pooledObects = new List<GameObject>();

    // generates a new object of specified type and returns it
    //      -> if inactive object of the same type already exists returns that object
    public GameObject GetObject(string type){

       //looks for the object with the same type in already existing pool of objects
       foreach (GameObject go in pooledObects)
       {
           if(go.name == type && !go.activeInHierarchy){ 
               go.SetActive(true);
               return go;
           }
       }
       //if not found, creates a new object and adds it to the object pool
       for (int i=0; i<objectPrefabs.Length; i++){
           if(objectPrefabs[i].name == type){
               Point spawnPoint = MapManager.Instance.SpawnPos;
               Vector3 worldCenterCoordinate = MapManager.Instance.getTiles()[spawnPoint].GetCenterWorldPosition();
               //GameObject newObject = Instantiate(objectPrefabs[i]);
               GameObject newObject = PhotonNetwork.Instantiate(objectPrefabs[i].name, worldCenterCoordinate, Quaternion.identity, 0, null);
               pooledObects.Add(newObject);
               newObject.name = type;
               return newObject;
           }
       }
       //if unsuccessful returns null
       return null;
   }

    //method that releases the object by making it inactive on the field
    //      -> removes it from the field
    public void ReleaseObject(GameObject gameObject){
        gameObject.SetActive(false);
    }

}
