using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform t;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Sprite[] s;
    
    void Awake() {
        Util.startup(this);
    }

    public void setPos(Vector2 v) {
        t.position = new Vector3(v.x, v.y, 0f);
    }

    public void move(Vector2 v) {
        Util.face(t.position, v, sr, s);
        setPos(v);
        GameManager.onPlayerAction();
    }
}
