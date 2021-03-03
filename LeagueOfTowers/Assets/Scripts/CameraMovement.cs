using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    // Start is called before the first frame update

    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

    private void Awake()
    {
       
    }


    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
        
        GetInput();
        Vector3 camPosition = this.transform.position;

        if (camPosition.x < minX)
        {
            camPosition.x = minX + 0.5f;
        }
        
        if(camPosition.x > maxX)
        {
            camPosition.x = maxX - 0.5f;
        }

        if(camPosition.y < minY)
        {
            camPosition.y = minY + 0.5f;
        }

        if(camPosition.y > maxY)
        {
            camPosition.y = maxY - 0.5f;
        }

        this.transform.position = camPosition;
    }

    private void GetInput()
    {

        if (Input.GetKey(KeyCode.W))//up
        {
            transform.Translate(new Vector3(0, 1, 0) * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))//down
        {
            transform.Translate(new Vector3(0, -1, 0) * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))//left
        {
            transform.Translate(new Vector3(-1, 0, 0) * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))//right
        {
            transform.Translate(new Vector3(1, 0, 0) * moveSpeed * Time.deltaTime);
        } 
    }

    public void SetBounders(float[] mapBounders)
    {
        this.minX = mapBounders[0] ;
        this.maxX = mapBounders[1] ;
        this.minY = mapBounders[2] ;
        this.maxY = mapBounders[3] ;
    }
}
