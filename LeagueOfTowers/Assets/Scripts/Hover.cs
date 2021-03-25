using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Hover : Singleton<Hover>
{
    // Start is called before the first frame update

    private SpriteRenderer spriteRenderer;
    private Camera playerCamera;

    private SpriteRenderer rangedSpriteRenderer;

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

    private void FollowMouse()
    {
        if(playerCamera != null){
            if (this.spriteRenderer.enabled)
            {
                //get mouse location and transform this Hover to the mouse location
                //this.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                this.transform.position = this.playerCamera.ScreenToWorldPoint(Input.mousePosition);

                //set z-coord to 0 so the camera can see it
                this.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            }
        }
        
    }

    // public void SetPlayerCamera(Camera playerCameraInUsing){
    //     this.playerCamera = playerCameraInUsing;
    // }

    public SpriteRenderer GetSpriteRenderer()
    {
        return this.spriteRenderer;
    }

    public void Activate(Sprite sprite, Camera playerCameraInUsing)
    {
        this.spriteRenderer.enabled = true;
        rangedSpriteRenderer.enabled = true;
        this.spriteRenderer.sprite = sprite;
        this.playerCamera = playerCameraInUsing;
    }

    public void Deactivate()
    {
        this.spriteRenderer.enabled = false;
        this.playerCamera = null;
        rangedSpriteRenderer.enabled = false;
    }
}
