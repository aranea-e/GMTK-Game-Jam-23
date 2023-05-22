using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform t;
    
    void Awake() {
        Util.startup(this);
    }

    public void setPos(Vector2 v) {
        t.position = new Vector3(v.x, v.y, 0f);
    }
}
