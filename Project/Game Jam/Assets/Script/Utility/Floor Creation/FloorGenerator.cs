using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FloorGenerator {
    public static List<Room> floor;
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
}
