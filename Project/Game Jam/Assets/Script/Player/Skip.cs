using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skip : MonoBehaviour {
    [SerializeField] Transform t;

    void Update() {
        Vector2[] skipBounds = findBounds();
        if (Input.GetMouseButtonDown(0) && Util.mouseInBounds(skipBounds[0], skipBounds[1])) {
            onClick();
        }
    }

    public Vector2[] findBounds() {
        Vector2[] bounds = new Vector2[2];
        bounds[0] = new Vector2(t.position.x - .325f, t.position.y - .325f);
        bounds[1] = new Vector2(t.position.x + .325f, t.position.y + .325f);
        return bounds;
    }

    public void onClick() {
        Util.getP().move(Util.getP().t.position);
        GameManager.onPlayerAction();
        GameManager.timer += .5f;
    }
}
