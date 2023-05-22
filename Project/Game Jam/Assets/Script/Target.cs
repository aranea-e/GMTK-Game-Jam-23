using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Color c;
    [SerializeField] Transform t;
    [SerializeField] string type;
    
    void Update() {
        sr.color = new Color(c.r, c.g, c.b, 
                             (Mathf.Sin(Time.realtimeSinceStartup * 2f) + 5/3f) * 0.375f);
        Vector2 low = new Vector2(t.position.x, t.position.y);
        Vector2 high = new Vector2(t.position.x + 1f, t.position.y + 1f);
        if (Input.GetMouseButtonDown(0) && Util.mouseInBounds(low, high)) {
            onClick();
        }
    }

    public void SetColor(Color c) {
        this.c = c;
    }

    public void Destroy() {
        Object.Destroy(gameObject);
    }

    public void onClick() {
        if (type == "move") {
            Util.getP().setPos(new Vector2(t.position.x, t.position.y));
        }
    }
}