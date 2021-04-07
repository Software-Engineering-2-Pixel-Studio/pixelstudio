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
    public void ToggleRangeSR()
    {
        mySpriteRenderer.enabled = !mySpriteRenderer.enabled;
    }

    //events
    public void OnTriggerEnter2D(Collider2D other)
    {
        if(this.myTowerScript != null){
            this.myTowerScript.OnTriggerEnter2DRange(other);
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if(this.myTowerScript != null){
            this.myTowerScript.OnTriggerExit2DRange(other);
        }
    }

}
