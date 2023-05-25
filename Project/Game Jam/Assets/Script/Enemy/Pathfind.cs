using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Pathfind {
    public static List<PathfindTarget> pts;
    public static List<Vector3> coords;

    public static Vector3 pathfind(Vector3 s, Vector3 t, int spd, int vran) {
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
        
        return result;
    }
}
