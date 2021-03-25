using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ObjectPool : MonoBehaviour
{
   [SerializeField]
   private GameObject[] objectPrefabs;

   private List<GameObject> pooledObects = new List<GameObject>();

   public GameObject GetObject(string type){

       foreach (GameObject go in pooledObects)
       {
           if(go.name == type && !go.activeInHierarchy){ 
               go.SetActive(true);
               return go;
           }
       }
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
       return null;
   }

   public void ReleaseObject(GameObject gameObject){
       gameObject.SetActive(false);
   }

}
