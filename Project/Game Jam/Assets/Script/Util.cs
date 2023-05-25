using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util {
    static Player p;

    public static void startup(Player player) {
        p = player;
    }
    
    public static Vector2 mousePosition() {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public static bool mouseInBounds(Vector2 low, Vector2 high) {
        return low.x <= mousePosition().x &&
               low.y <= mousePosition().y &&
               high.x >= mousePosition().x &&
               high.y >= mousePosition().y;
    }
    public static Player getP() {
        return p;
    }

    public static bool creatureOccupying(Vector3 v) {
        foreach (Enemy e in Object.FindObjectsOfType<Enemy>()) {
            if (e.t.position == v) {
                return true;
            }
        }
        return false;
    }

    public static bool tileAvailable(Vector3 v) {
        return (!GameManager.world[0].HasTile(Vector3Int.FloorToInt(v)) 
                && GameManager.world[1].HasTile(Vector3Int.FloorToInt(v))
                && !creatureOccupying(v));
    }

    public static void face(Vector2 start, Vector2 target, SpriteRenderer sr, Sprite[] s) {
        if (target.x > start.x) {
            sr.sprite = s[2];
        } else if (target.x < start.x) {
            sr.sprite = s[3];
        }
        if (target.y > start.y) {
            sr.sprite = s[1];
        } else if (target.y < start.y) {
            sr.sprite = s[0];
        }
    }

    public static void face(Vector2 direction, SpriteRenderer sr, Sprite[] s) {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) {
            if (direction.x < 0) {
                sr.sprite = s[2];
            } else {
                sr.sprite = s[3];
            }
        } else {
            if (direction.y < 0) {
                sr.sprite = s[1];
            } else {
                sr.sprite = s[0];
            }
        }
    }
}
