using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/*
*   Code shamelessly ripped from:
*   https://forum.unity.com/threads/click-drag-camera-movement.39513/
*/

public class DragCamera : MonoBehaviour {
    public float dragSpeed;
    private Vector3 dragOrigin;
    public Tilemap tmp;
 
 
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            dragOrigin = Input.mousePosition;
            return;
        }
 
        if (!Input.GetMouseButton(1)) return;
 
        Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        Vector3 move = new Vector3(-pos.x * dragSpeed, -pos.y * dragSpeed, 0f);
 
        transform.Translate(move, Space.World);  

        if (transform.position.x < tmp.localBounds.min.x) { transform.position = new Vector3(tmp.localBounds.min.x, transform.position.y, transform.position.z); }
        if (transform.position.y < tmp.localBounds.min.y) { transform.position = new Vector3(transform.position.x, tmp.localBounds.min.y, transform.position.z); }
        if (transform.position.x > tmp.localBounds.max.x) { transform.position = new Vector3(tmp.localBounds.max.x, transform.position.y, transform.position.z); }
        if (transform.position.y > tmp.localBounds.max.y) { transform.position = new Vector3(transform.position.x, tmp.localBounds.max.y, transform.position.z); }
    }
}
