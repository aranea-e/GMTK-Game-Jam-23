using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Pathfind {
    public static List<PathfindTarget> pts;
    public static List<Vector3> coords;
    public static Vector3 target;
    public static Enemy current;
    public static int distance;
    public static int targetRange;

    public static Vector3 pathfind(Vector3 s, Vector3 t, int spd, int vran, int ran, Enemy c, bool attack) {
        if (Vector3.Distance(s, t) > vran) {
            return s;
        }

        targetRange = vran;
        target = t;
        current = c;
        pts = new List<PathfindTarget>();
        coords = new List<Vector3>();

        pts.Add(new PathfindTarget(t, null, 0));
        coords.Add(t);
        pts[0].pathfindFromTarget();

        Vector3 result = s;
        PathfindTarget resultT = null;
        for (int i = 0; i < pts.Count; i++) {
            if (coords[i] == s && coords[i] != t && pts[i].distance < vran + 1) {
                result = coords[i];
                resultT = pts[i];
                distance = pts[i].distance;
                for (int j = 0; j < spd; j++) {
                    if (resultT.from != null && resultT.from.pos != t) {
                        resultT = resultT.from;
                        result = resultT.pos;
                    }
                }
            }
        }

        if (resultT == null) {
            return s;
        }

        if (distance - 1 < ran && Util.lineOfSight(s, t)) {
            if (attack && current.hp > 0) {
                current.attack(target);
            } else {
                return t;
            }
            Util.face(s - t, current.sr, current.s);
            return s;
        }

        return result;
    }
}
