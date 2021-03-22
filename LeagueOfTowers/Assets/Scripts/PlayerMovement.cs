using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //fields
    [SerializeField] private float speed;

    // Start is called before the first frame update
    private void Start()
    {
        this.speed = 10;
    }

    // Update is called once per frame
    private void Update()
    {
        
        //if these are not specific who is the local player, all players on scene will
        //move when anyone hit the keyboard
        //get input from keyboard
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        //speed * Time.deltaTime = distance -> Find the destination from a distance which direction
        Vector2 moveAmount = moveInput.normalized * speed * Time.deltaTime;

        //Actually move to the destination on scene
        transform.position += (Vector3)moveAmount;
      
    }
}
