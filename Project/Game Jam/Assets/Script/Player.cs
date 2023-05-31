using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public Transform t;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Transform srt;
    [SerializeField] Sprite[] s;
    Vector3 last;
    public int ran;
    [SerializeField] Sprite atks;
    
    void Awake() {
        last = t.position;
        Util.startup(this);
    }

    [ContextMenu("stay")]
    void stay() {
        move(t.position);
    }

    public void Update() {
        GameManager.timer += Time.deltaTime;
        if (GameManager.timer >= 1f) {
            GameManager.timer = 1f;
            if (GameManager.targetInstances == null) {
                GameManager.createMoveTargets();
            }
        } else {
            GameManager.destroyAllTargets();
        }
        
        if (GameManager.timer < 0.5f) {
            Util.moveSprite(srt, last, t.position, GameManager.timer);
        } else {
            Util.moveSprite(srt, last, t.position, 0.5f);
        }
    }

    public void setPos(Vector2 v) {
        last = t.position;
        t.position = new Vector3(v.x, v.y, 0f);
    }

    public void move(Vector2 v) {
        Util.face(t.position, v, sr, s);
        setPos(v);
        GameManager.onPlayerAction();
        Util.moveSprite(srt, last, t.position, 0f);
    }

    public void attack(Vector2 v) {
        Util.face(t.position, v, sr, s);
        last = t.position;
        GameManager.attackAnim((ran > 1), true, atks, t.position, v);
        GameManager.onPlayerAction();
    }
}
