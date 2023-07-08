using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindShortcutNode {
    public Room r;
    public PathfindShortcutNode from;
    public int distance;
    public bool nodesCreated = false;
    public Vector2 entrance;

    public PathfindShortcutNode(Room r, PathfindShortcutNode from, int distance) {
        this.r = r;
        this.from = from;
        this.distance = distance;
        entrance = Vector2.zero;
    }

    public void createNewNodes() {
        if (nodesCreated) {
            return;
        }
        nodesCreated = true;
        Vector2[] coords = {r.pos + new Vector2(0, 1),
                            r.pos + new Vector2(1, 0),
                            r.pos + new Vector2(0, -1),
                            r.pos + new Vector2(-1, 0)};
        for (int i = 0; i < 4; i++) {
            Vector2 pos = coords[i];
            if (FloorGenerator.isRoomAtCoords(pos) && r.doors[i]) {
                if (FloorGenerator.roomAvailable(FloorGenerator.roomAtCoords(pos))) {
                    PathfindShortcutNode gen = new PathfindShortcutNode(FloorGenerator.roomAtCoords(pos), this, distance + 1);
                    FloorGenerator.PSNs.Add(gen);
                } else if (FloorGenerator.nodeAt(FloorGenerator.roomAtCoords(pos)).distance > distance + 1) {
                    FloorGenerator.nodeAt(FloorGenerator.roomAtCoords(pos)).from = this;
                    FloorGenerator.nodeAt(FloorGenerator.roomAtCoords(pos)).distance = distance + 1;
                }
            }
        }
    }

    public void findEntrance(PathfindShortcutNode PSN) {
        Vector2 mod = Vector2.zero;
        if (PSN.r.pos.x > r.pos.x) {
            mod = new Vector2(3, 0);
        }
        if (PSN.r.pos.x < r.pos.x) {
            mod = new Vector2(-3, 0);
        }
        if (PSN.r.pos.y > r.pos.y) {
            mod = new Vector2(0, 3);
        }
        if (PSN.r.pos.y < r.pos.y) {
            mod = new Vector2(0, -3);
        }
        entrance = (r.pos * new Vector2(7, 7)) + mod;
    }
}
