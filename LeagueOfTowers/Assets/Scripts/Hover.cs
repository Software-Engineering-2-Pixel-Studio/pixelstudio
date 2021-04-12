using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Hover : Singleton<Hover>
{
    //fields
    private SpriteRenderer spriteRenderer;      //tower's image
    private Camera playerCamera;      //player's camera
    private SpriteRenderer rangedSpriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();
        this.spriteRenderer.enabled = false;

        this.rangedSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        FollowMouse();
    }

    /*
        Method to make this Hover object to follow the mouse
    */
    private void FollowMouse()
    {
        if(playerCamera != null){
            if (this.spriteRenderer.enabled)
            {
                //get mouse location and transform this Hover to the mouse location
                this.transform.position = this.playerCamera.ScreenToWorldPoint(Input.mousePosition);

                //set z-coord to 0 so the camera can see it
                this.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            }
        }
        
    }

    /*
        Method to get image that this Hover is holding
    */
    public SpriteRenderer GetSpriteRenderer()
    {
        return this.spriteRenderer;
    }

    /*
        Method to display components of this Hover on scene
    */
    public void Activate(Sprite sprite, Camera playerCameraInUsing)
    {
        this.spriteRenderer.enabled = true;
        rangedSpriteRenderer.enabled = true;
        this.spriteRenderer.sprite = sprite;
        this.playerCamera = playerCameraInUsing;
    }

    /*
        Method to hide components of this Hover on scene
    */
    public void Deactivate()
    {
        this.spriteRenderer.enabled = false;
        this.playerCamera = null;
        rangedSpriteRenderer.enabled = false;
    }
}
