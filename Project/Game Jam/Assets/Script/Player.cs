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

    public void move(Vector2 v) {
        setPos(v);
        GameManager.onPlayerAction();
    }
}
