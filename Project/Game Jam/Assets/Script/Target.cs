using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Transform t;
    public TargetSO so;
    
    void Update() {
        sr.color = new Color(so.c.r, so.c.g, so.c.b, 
                             (Mathf.Cos(Time.realtimeSinceStartup * 2f) + 5/3f) * 0.375f);
        Vector2 low = new Vector2(t.position.x, t.position.y);
        Vector2 high = new Vector2(t.position.x + 1f, t.position.y + 1f);
        if (Input.GetMouseButtonDown(0) && Util.mouseInBounds(low, high)) {
            onClick();
        }
    }

    public void Destroy() {
        Object.Destroy(gameObject);
    }

    public void onClick() {
        if (so.type == "move") {
            Util.getP().move(new Vector2(t.position.x, t.position.y));
        }
    }
}