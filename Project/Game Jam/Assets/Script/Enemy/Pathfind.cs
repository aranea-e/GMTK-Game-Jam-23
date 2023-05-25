using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Pathfind {
    public static List<PathfindTarget> pts;
    public static List<Vector3> coords;
    public static Vector3 target;

    public static Vector3 pathfind(Vector3 s, Vector3 t, int spd, int vran, int ran) {
        target = t;
        pts = new List<PathfindTarget>();
        coords = new List<Vector3>();

        pts.Add(new PathfindTarget(t, null, 0));
        coords.Add(t);
        pts[0].pathfindFromTarget();

        Vector3 result = s;
        PathfindTarget resultT = null;
        for (int i = 0; i < pts.Count; i++) {
            if (coords[i] == s && pts[i].distance < vran + 1) {
                result = coords[i];
                resultT = pts[i];
                for (int j = 0; j < spd; j++) {
                    if (resultT.from != null) {
                        resultT = resultT.from;
                        result = resultT.pos;
                    }
                }
            }
        }

        if (resultT == null) {
            return s;
        }

        if (resultT.distance < ran && lineOfSight(s, t)) {
            Debug.Log("attack");
            return s;
        }

        return result;
    }

    static bool lineOfSight(Vector2 s, Vector2 t) {
        s = new Vector2(s.x + .5f, s.y + .5f);
        t = new Vector2(t.x + .5f, t.y + .5f);
        Vector2 direction = t - s;
        RaycastHit2D result = Physics2D.Raycast(s, direction, Vector2.Distance(s, t), GameManager.wallLM);
        return result.collider == null;
    }
}
