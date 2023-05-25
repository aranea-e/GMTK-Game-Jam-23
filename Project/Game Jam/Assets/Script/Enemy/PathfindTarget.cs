using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindTarget {
    public Vector3 pos;
    public PathfindTarget to;
    public PathfindTarget from;
    public int distance;

    public PathfindTarget(Vector3 pos, PathfindTarget from, int distance) {
        this.pos = pos;
        this.from = from;
        this.distance = distance;
    }

    public void pathfindFromTarget() {
        Vector3[] coords = {new Vector3(pos.x, pos.y + 1, 0),
                            new Vector3(pos.x, pos.y - 1, 0),
                            new Vector3(pos.x + 1, pos.y, 0),
                            new Vector3(pos.x - 1, pos.y, 0)};
        PathfindTarget[] createdPTs = new PathfindTarget[4];
        for (int i = 0; i < 4; i++) {
            if (Pathfind.coords.Contains(coords[i])) {
                createdPTs[i] = replaceTarget(coords[i]);
            } else if (Util.tileAvailable(coords[i])) {
                createdPTs[i] = new PathfindTarget(coords[i], this, distance + 1);
                Pathfind.coords.Add(coords[i]);
                Pathfind.pts.Add(createdPTs[i]);
            }
        }
        foreach (PathfindTarget p in createdPTs) {
            if (p != null) {
                p.pathfindFromTarget();
            }
        }
    }

    PathfindTarget replaceTarget(Vector3 v) {
        for (int i = 0; i < Pathfind.coords.Count; i++) {
            if (Pathfind.coords[i].Equals(v)) {
                if (Pathfind.pts[i].distance > this.distance + 1) {
                    Pathfind.pts[i].distance = distance + 1;
                    Pathfind.pts[i].from = this;
                    return Pathfind.pts[i];
                } else if (Pathfind.pts[i].distance.Equals(this.distance + 1) 
                            && Vector3.Distance(Pathfind.target, Pathfind.pts[i].from.pos) > 
                               Vector3.Distance(Pathfind.target, pos)) {
                    Pathfind.pts[i].distance = distance + 1;
                    Pathfind.pts[i].from = this;
                    return Pathfind.pts[i];
                }
            }
        }
        return null;
    }
}
