using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util {
    static Player p;
    static Transform door;

    public static void startup(Player player, Transform d) {
        p = player;
        door = d;
    }
    
    public static Vector2 mousePosition() {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    public static Vector2 mousePositionInt() {
        return new Vector2(Mathf.Floor(mousePosition().x), Mathf.Floor(mousePosition().y));
    }

    public static float screenBottom() {
        return Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane)).y;
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

    public static Transform getD() {
        return door;
    }

    public static bool creatureOccupying(Vector3 v) {
        foreach (Enemy e in Object.FindObjectsOfType<Enemy>()) {
            if (e.t.position == v) {
                return true;
            }
        }
        return false;
    }

    public static Enemy creatureAt(Vector3 v) {
        foreach (Enemy e in Object.FindObjectsOfType<Enemy>()) {
            if (e.t.position == v) {
                return e;
            }
        }
        return null;
    }

    public static Item itemOccupying(Vector3 v) {
        foreach (Item i in Object.FindObjectsOfType<Item>()) {
            if (i.t.position == v) {
                return i;
            }
        }
        return null;
    }

    public static Weapon slotAt(Vector2 v) {
        return p.inven.slotAt(v);
    }

    public static bool enemiesMoving() {
        foreach (Enemy e in Object.FindObjectsOfType<Enemy>()) {
            if (e.t.position != e.last) {
                return true;
            }
        }
        foreach (AttackAnim a in Object.FindObjectsOfType<AttackAnim>()) {
            if (!a.player) {
                return true;
            }
        }
        return false;
    }

    public static bool tileAvailable(Vector3 v) {
        return (!GameManager.world[0].HasTile(Vector3Int.FloorToInt(v)) 
                && GameManager.world[1].HasTile(Vector3Int.FloorToInt(v))
                && !creatureOccupying(v) && !isDoor(v));
    }

    public static bool tileAvailableIgnoreEnemies(Vector3 v) {
        return (!GameManager.world[0].HasTile(Vector3Int.FloorToInt(v)) 
                && GameManager.world[1].HasTile(Vector3Int.FloorToInt(v))
                && !isDoor(v));
    }

    public static bool isDoor(Vector3 v) {
        return (v == door.position);
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

    public static void moveSprite(Transform srt, Vector3 from, Vector3 to, float t) {
        Vector3 mod = (to - from) * (calculateMod(t));
        srt.position = from + mod + new Vector3(0.5f, 0.5f, 0);
    }

    public static float calculateMod(float t) {
        return (-Mathf.Cos(2 * Mathf.PI * t) / 2) + 0.5f;
    }

    public static bool lineOfSight(Vector2 s, Vector2 t) {
        s = new Vector2(s.x + .5f, s.y + .5f);
        t = new Vector2(t.x + .5f, t.y + .5f);
        return raycastHit(s, t) ||
               raycastHit(s + new Vector2(3/16f, 0), t) ||
               raycastHit(s + new Vector2(0, 3/16f), t) ||
               raycastHit(s + new Vector2(-3/16f, 0), t) ||
               raycastHit(s + new Vector2(0, -3/16f), t);
    }

    static bool raycastHit(Vector2 s, Vector2 t) {
        Vector2 direction = t - s;
        RaycastHit2D result = Physics2D.Raycast(s, direction, Vector2.Distance(s, t), GameManager.wallLM);
        return result.collider == null;
    }
}
