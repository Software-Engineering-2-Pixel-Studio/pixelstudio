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

        //limit the movement of player(a camera holder for each player)
        float clampedX = Mathf.Clamp(transform.position.x, 0, 0.12f);
        float clampedY = Mathf.Clamp(transform.position.y, 0, 0);
        transform.position = Vector2.Lerp(transform.position, new Vector2(clampedX, clampedY), speed);
      
    }
}
