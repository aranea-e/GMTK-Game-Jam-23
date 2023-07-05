using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Transform t;
    public TargetSO so;
    float timeSinceSpawn;
    
    void Update() {
        timeSinceSpawn += Time.deltaTime;
        sr.color = new Color(so.c.r, so.c.g, so.c.b, 
                             (Mathf.Cos(timeSinceSpawn * 2f) + 5/3f) * 0.375f);
        Vector2 low = new Vector2(t.position.x, t.position.y);
        Vector2 high = new Vector2(t.position.x + 1f, t.position.y + 1f);
        Vector2[] skipBounds = FindObjectOfType<Skip>().findBounds();
        if (Input.GetMouseButtonDown(0) && Util.mouseInBounds(low, high)
            && !Util.mouseInBounds(skipBounds[0], skipBounds[1])) {
            onClick();
        }
    }

    public void Update(bool f) {
        timeSinceSpawn += Time.deltaTime;
        sr.color = new Color(so.c.r, so.c.g, so.c.b, 
                             (Mathf.Cos(timeSinceSpawn * 2f) + 5/3f) * 0.375f);
        Vector2 low = new Vector2(t.position.x, t.position.y);
        Vector2 high = new Vector2(t.position.x + 1f, t.position.y + 1f);
    }

    public void Destroy() {
        Object.Destroy(gameObject);
    }

    public void onClick() {
        switch (so.type)
        {
            case "move":
                Util.getP().move(t.position);
                break;
            
            case "attack":
                Util.getP().attack(t.position);
                break;
        }
        GameManager.onPlayerAction();
    }
}