using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FloorGenerator {
    public static List<Room> floor;
    public static List<PathfindShortcutNode> PSNs;
    public static GameObject[] visualizers;
    public static bool doorRoomMade;
    public static bool lastRound;
    public static int size;

    public static void createFloor(int s) {
        size = s;
        floor = new List<Room>();
        lastRound = false;
        doorRoomMade = false;
        floor.Add(new Room(new Vector2(0, 0), true));
        for (int i = 0; i < s; i++) {
            lastRound = (i == s - 1);
            int count = floor.Count;
            for (int j = 0; j < count; j++) {
                floor[j].createNewRooms();
            }
        }

        if (floor.Count < s * 2.5 || !findDoor()) {
            createFloor(s);
        }
    }

    static bool findDoor() {
        for (int i = 0; i < floor.Count; i++) {
            if (floor[i].startOrEnd == 2) {
                return true;
            }
        }
        return false;
    }

    public static string FGDebug() {
        string result = "";
        for (int i = 0; i < floor.Count; i++) {
            result += "Room " + i + ":\n" + floor[i];
        }
        return result;
    }

    public static bool isRoomAtCoords(Vector2 coords) {
        for (int i = 0; i < floor.Count; i++) {
            if (coords == floor[i].pos) {
                return true;
            }
        }
        return false;
    }
    public static Room roomAtCoords(Vector2 coords) {
        for (int i = 0; i < floor.Count; i++) {
            if (coords == floor[i].pos) {
                return floor[i];
            }
        }
        return null;
    }

    public static Vector3 pathfindShortcut(Vector3 s, Vector3 t) {
        Room troom = roomContaining(t);
        Room sroom = roomContaining(s);
        if (troom == null || sroom == null) {
            return s;
        }
        PSNs = new List<PathfindShortcutNode>();
        PSNs.Add(new PathfindShortcutNode(troom, null, 0));
        PSNs[0].createNewNodes();

        bool found = false;
        while (!found) {
            int count = PSNs.Count;
            for (int i = 0; i < count; i++) {
                if (PSNs[i].r == sroom) {
                    found = true;
                    i = count;
                } else {
                    PSNs[i].createNewNodes();
                }
            }
        }

        PathfindShortcutNode tPSN = null;
        for (int i = 0; i < PSNs.Count; i++) {
            if (sroom == PSNs[i].r) {
                tPSN = PSNs[i];
            }
        }
        if (tPSN.from != null) {
            tPSN = tPSN.from;
        }

        return (tPSN.r.pos * new Vector2(7, 7));
    }

    static bool isTileInRoom(Vector3 t, Room r) {
        return (t.x >= r.pos.x * 7 - 3) && (t.x <= r.pos.x * 7 + 3) 
               && (t.y >= r.pos.y * 7 - 3) && (t.y <= r.pos.y * 7 + 3);
    }

    static Room roomContaining(Vector3 t) {
        foreach (Room r in floor) {
            if (isTileInRoom(t, r)) {
                return r;
            }
        }
        return null;
    }

    public static bool areInSameRoom(Vector3 s, Vector3 t) {
        Room sroom = roomContaining(s);
        return isTileInRoom(t, sroom);
    }

    public static bool roomAvailable(Room r) {
        foreach (PathfindShortcutNode PSN in PSNs) {
            if (r == PSN.r) {
                return false;
            }
        }
        return true;
    }

    public static PathfindShortcutNode nodeAt(Room r) {
        foreach (PathfindShortcutNode PSN in PSNs) {
            if (r == PSN.r) {
                return PSN;
            }
        }
        return null;
    }
}
