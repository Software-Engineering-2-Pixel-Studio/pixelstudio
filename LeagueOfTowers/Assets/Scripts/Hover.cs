using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : Singleton<Hover>
{
    // Start is called before the first frame update

    private SpriteRenderer spriteRenderer;
    void Start()
    {
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();
        this.spriteRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        FollowMouse();
    }

    private void FollowMouse()
    {
        if (this.spriteRenderer.enabled)
        {
            //get mouse location and transform this Hover to the mouse location
            this.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //set z-coord to 0 so the camera can see it
            this.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
        
    }

    public SpriteRenderer GetSpriteRenderer()
    {
        return this.spriteRenderer;
    }

    public void Activate(Sprite sprite)
    {
        this.spriteRenderer.enabled = true;
        this.spriteRenderer.sprite = sprite;
    }

    public void Deactivate()
    {
        this.spriteRenderer.enabled = false;
    }
}
