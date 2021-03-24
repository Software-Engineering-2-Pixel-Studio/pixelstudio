using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Monster : MonoBehaviour
{
    [SerializeField]
    private float speed; //serialized to access from different places
    private Stack<Node> path;

    public Point GridPosition{ get; set;}

    private Vector3 destination;

    public bool IsActive{ get; set; }

    private void Update(){
        Move();
    }

    public void SetActive(bool value){
        IsActive = value;
    }

    //Spawns a monster on a map by setting it's position first to the position of the 
    //   Spawn portal
    public void Spawn(){
            if(this.name == "TrainingDummy"){
                StartCoroutine(Scale(new Vector3(0.1f, 0.1f), new Vector3(0.6f,0.6f), false));
                transform.position = MapManager.Instance.SpawnPrefab.transform.position;
            }
            else if(this.name == "Scarecrow"){
                StartCoroutine(Scale(new Vector3(0.1f, 0.1f), new Vector3(0.5f,0.5f), false));
                transform.position = MapManager.Instance.SpawnPrefab.transform.position;
            }

            SetPath(MapManager.Instance.Path);
    }

    public IEnumerator Scale(Vector3 from, Vector3 to, bool destroy){
        //IsActive = false;

        float progress = 0;

        while (progress<=1)
        {
            transform.localScale = Vector3.Lerp(from, to, progress);
            progress += Time.deltaTime;
            yield return null;
        }
        
        //to make sure it is exactly equal to *to*
        transform.localScale = to;

        IsActive = true;

        //in case we need to release the moster and make it inactive
        if(destroy){
            Release();
        }
    }

    private void Move(){
        if(IsActive){
            transform.position = Vector2.MoveTowards(transform.position, destination, speed*Time.deltaTime);

            if(transform.position == destination){
                if(path != null && path.Count > 0){
                    GridPosition = path.Peek().GridPosition;
                    destination = path.Pop().WorldPosition;
                }
            }
        }
    }

    private void SetPath(Stack<Node> newPath){
        if(newPath != null){
            this.path = newPath;
            GridPosition = path.Peek().GridPosition;
            destination = path.Pop().WorldPosition;
        }
        
    }

    //to 
    private void OnTriggerEnter2D(Collider2D other){
        //if the monster collide with the base
        if(other.tag == "BasePortal"){
            //scale down in size them
            if(this.name == "TrainingDummy"){
                StartCoroutine(Scale(new Vector3(0.6f, 0.6f), new Vector3(0.1f,0.1f), true));
            }
            else if(this.name == "Scarecrow"){
                StartCoroutine(Scale(new Vector3(0.5f, 0.5f), new Vector3(0.1f,0.1f), true));
            }
        }
    }

    private void Release(){
        IsActive = false; // so next time we use the object it starts as not active;
        GridPosition = MapManager.Instance.SpawnPos; // to make sure next time we use the object it starts at start position
        GameManager.Instance.Pool.ReleaseObject(gameObject);    // makes an object inactive for later usage
        GameManager.Instance.removeMonster(this);   // removes the monster from the "active monsters of the wave" list
    }
}
