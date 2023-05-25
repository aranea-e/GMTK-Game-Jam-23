using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] EnemySO so;
    [SerializeField] Transform t;
    float timer;

    void Awake() {
        switch (so.aiType) {
            case "basic":
                so.ai = new Basic();
                break;
            
            default:
                so.ai = new AI();
                break;
        }
    }

    void Update() {
        timer += Time.deltaTime;
        if (timer > 1) {
            pathfind();
            timer = 0;
        }
    }

    [ContextMenu("pathfind")]
    public void pathfind() {
        t.position = so.ai.pathfind(t.position, Util.getP().t.position, so.spd, so.vran);
    }
}
