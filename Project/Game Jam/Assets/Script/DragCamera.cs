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
    public bool cameraControl;
    public Vector3 cameraTarget;
    public Vector3 origTarget;
    public float timeSinceTarget;
    public float zoomTransition;
 
    void Update()
    {
        if (cameraControl) {
            zoomTransition += Time.deltaTime * 2;
            if (zoomTransition > 1f) {
                zoomTransition = 1f;
            }
            Camera.main.orthographicSize = 3 + (4.5f * zoomTransition);
            if (Input.GetMouseButtonDown(1))
            {
                dragOrigin = Util.mousePosition();
                return;
            }
    
            if (!Input.GetMouseButton(1)) return;
    
            Vector3 pos = Util.mousePosition() - (Vector2) dragOrigin;
            Vector3 move = new Vector3(-pos.x * dragSpeed, -pos.y * dragSpeed, 0f);
    
            transform.Translate(move, Space.World);  

            if (transform.position.x < tmp.localBounds.min.x) { transform.position = new Vector3(tmp.localBounds.min.x, transform.position.y, transform.position.z); }
            if (transform.position.y < tmp.localBounds.min.y) { transform.position = new Vector3(transform.position.x, tmp.localBounds.min.y, transform.position.z); }
            if (transform.position.x > tmp.localBounds.max.x) { transform.position = new Vector3(tmp.localBounds.max.x, transform.position.y, transform.position.z); }
            if (transform.position.y > tmp.localBounds.max.y) { transform.position = new Vector3(transform.position.x, tmp.localBounds.max.y, transform.position.z); }
            origTarget = transform.position;
        } else {
            zoomTransition -= Time.deltaTime * 2;
            if (zoomTransition < 0f) {
                zoomTransition = 0f;
            }
            Camera.main.orthographicSize = 3 + (4.5f * zoomTransition);
            timeSinceTarget += Time.deltaTime;
            if (timeSinceTarget > 0.5f) {
                timeSinceTarget = 0.5f;
            }

            float origPercent = (0.5f - timeSinceTarget) / 0.5f;
            float percent = timeSinceTarget / 0.5f;

            Vector3 final = origTarget * origPercent + cameraTarget * percent;
            transform.position = final;
        }
    }
}
