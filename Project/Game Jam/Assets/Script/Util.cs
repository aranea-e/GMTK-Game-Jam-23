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

    public static bool tileAvailable(Vector3 v) {
        return (!GameManager.world[0].HasTile(Vector3Int.FloorToInt(v)) 
                && GameManager.world[1].HasTile(Vector3Int.FloorToInt(v)));
    }
}
