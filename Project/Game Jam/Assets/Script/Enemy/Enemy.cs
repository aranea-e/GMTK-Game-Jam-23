using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] EnemySO so;
    public Transform t;
    public SpriteRenderer sr;
    public Sprite[] s;
    float timer;

    [ContextMenu("pathfind")]
    public void pathfind() {
        moveTo(Pathfind.pathfind(t.position, Util.getP().t.position, so.spd, so.vran, so.ran, this));
    }

    void moveTo(Vector3 p) {
        Util.face(t.position, p, sr, s);
        t.position = p;
    }
}
