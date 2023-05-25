using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform t;
    public SpriteRenderer s;
    public Sprite[] sprites;
    
    void Awake() {
        Util.startup(this);
    }

    public void setPos(Vector2 v) {
        t.position = new Vector3(v.x, v.y, 0f);
    }

    public void move(Vector2 v) {
        if (v.x > t.position.x) {
            face(2);
        } else if (v.x < t.position.x) {
            face(3);
        }
        if (v.y > t.position.y) {
            face(1);
        } else if (v.y < t.position.y) {
            face(0);
        }
        setPos(v);
        GameManager.onPlayerAction();
    }

    public void face(int d) {
        s.sprite = sprites[d];
    }
}
