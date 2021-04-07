using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeScript : MonoBehaviour
{
    //fields
    private SpriteRenderer mySpriteRenderer;
    private TowerScript myTowerScript;

    // Start is called before the first frame update
    void Start()
    {
        this.mySpriteRenderer = this.GetComponent<SpriteRenderer>();
        this.myTowerScript = this.GetComponentInParent<TowerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //actions
    /*
        toggle the sprite renderer for range
    */
    public void ToggleRangeSR()
    {
        mySpriteRenderer.enabled = !mySpriteRenderer.enabled;
    }

    //events
    /*
        this method is called when an enemy collide with the range
    */
    public void OnTriggerEnter2D(Collider2D other)
    {
        if(this.myTowerScript != null){
            this.myTowerScript.OnTriggerEnter2DRange(other);
        }
    }

    /*
        this method is called when an enemy get out of the range
    */
    public void OnTriggerExit2D(Collider2D other)
    {
        if(this.myTowerScript != null){
            this.myTowerScript.OnTriggerExit2DRange(other);
        }
    }

}
