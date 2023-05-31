using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemySO so;
    public Transform t;
    public Transform srt;
    public SpriteRenderer sr;
    public Sprite[] s;
    float timer;
    Vector3 last;

    void Start() {
        last = t.position;
    }

    public void Update() {
        if (GameManager.timer >= 0.5f) {
            Util.moveSprite(srt, last, t.position, GameManager.timer - 0.5f);
        } else {
            Util.moveSprite(srt, last, t.position, 0f);
        }
    }
    
    public void pathfind() {
        moveTo(Pathfind.pathfind(t.position, Util.getP().t.position, so.spd, so.vran, so.ran, this, true));
    }

    void moveTo(Vector3 p) {
        last = t.position;
        Util.face(t.position, p, sr, s);
        t.position = p;
    }

    public void attack(Vector3 target) {
        GameManager.attackAnim((so.ran > 1), false, so.atks, t.position, target);
    }
}
