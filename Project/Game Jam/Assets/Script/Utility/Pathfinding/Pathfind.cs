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
    public static bool ignoreEnemies;

    public static Vector3 pathfind(Vector3 s, Vector3 t, int spd, int vran, int ran, Enemy c, bool attack, bool iE) {
        if (Vector3.Distance(s, t) > vran) {
            return s;
        }

        ignoreEnemies = iE;
        targetRange = vran;
        target = t;
        current = c;
        pts = new List<PathfindTarget>();
        coords = new List<Vector3>();

        Vector3[] coordsToCheck = {new Vector3(s.x, s.y + 1, 0),
                            new Vector3(s.x, s.y - 1, 0),
                            new Vector3(s.x + 1, s.y, 0),
                            new Vector3(s.x - 1, s.y, 0)};

        Vector3 result = s;
        PathfindTarget resultT = null;

        bool found = false;
        for (int i = 0; i < 4; i++) {
            if (coordsToCheck[i] == t) {
                found = true;
            }
        }

        if (found && vran > 0 && spd > 0) {
            result = t;
        } else {
            pts.Add(new PathfindTarget(t, null, 0));
            coords.Add(t);
            pts[0].pathfindFromTarget();

            for (int i = 0; i < pts.Count; i++) {
                if (coords[i] == s && coords[i] != t && pts[i].distance < vran) {
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
        }

        if (found) { distance = 1; }
        Debug.Log(distance + " " + ran + " " + Util.lineOfSight(s, t) + " " + attack);
        if (distance <= ran && Util.lineOfSight(s, t) && attack) {
            if (current != null && current.hp > 0) {
                current.attack(target);
                Util.face(s - t, current.sr, current.s);
            } else if (current == null) {
                Util.getP().attack(target);
            } else {
                return t;
            }
            return s;
        }

        return result;
    }

    public static bool isInRange(Vector3 s, Vector3 t, int ran) {
        if (Vector3.Distance(s, t) > ran) {
            return false;
        }

        target = t;
        pts = new List<PathfindTarget>();
        coords = new List<Vector3>();

        Vector3[] coordsToCheck = {new Vector3(s.x, s.y + 1, 0),
                            new Vector3(s.x, s.y - 1, 0),
                            new Vector3(s.x + 1, s.y, 0),
                            new Vector3(s.x - 1, s.y, 0)};

        bool found = false;
        for (int i = 0; i < 4; i++) {
            if (coordsToCheck[i] == t) {
                found = true;
            }
        }

        if (found && ran >= 1) {
            return true;
        } else {
            pts.Add(new PathfindTarget(t, null, 0));
            coords.Add(t);
            pts[0].pathfindFromTarget();

            for (int i = 0; i < pts.Count; i++) {
                if (coords[i] == s && pts[i].distance <= ran) {
                    return true;
                }
            }
        }
        return false;
    }
}
