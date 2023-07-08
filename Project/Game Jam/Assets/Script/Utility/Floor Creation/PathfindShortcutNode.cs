using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindShortcutNode {
    public Room r;
    public PathfindShortcutNode from;
    public int distance;
    public bool nodesCreated = false;

    public PathfindShortcutNode(Room r, PathfindShortcutNode from, int distance) {
        this.r = r;
        this.from = from;
        this.distance = distance;
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
}
