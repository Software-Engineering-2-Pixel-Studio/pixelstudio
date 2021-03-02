using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform camTransform;
    [SerializeField] private float speed;

    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

    private float widthCameraBox;
    private float heightCameraBox;

    private void Awake()
    {
        float xCoord_BottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        float xCoord_BottomRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        widthCameraBox = xCoord_BottomRight - xCoord_BottomLeft;

        float yCoord_BottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;
        float yCoord_TopLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;
        heightCameraBox = yCoord_TopLeft - yCoord_BottomLeft;
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.position = camTransform.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(camTransform != null)
        {
            float clampedX = Mathf.Clamp(camTransform.position.x, minX, maxX);
            float clampedY = Mathf.Clamp(camTransform.position.y, minY, maxY);
            transform.position = Vector2.Lerp(transform.position, new Vector2(clampedX, clampedY), speed);
        }
    }
    public void SetBounders(float[] mapBounders)
    {
        this.minX = mapBounders[0] - (-widthCameraBox / 2);
        this.maxX = mapBounders[1] - (widthCameraBox / 2);
        this.minY = mapBounders[2] - (-heightCameraBox / 2);
        this.maxY = mapBounders[3] - (heightCameraBox / 2);
    }
}
