using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Room {
    public bool[] doors;
    public GameObject template;
    public Vector2 pos;
    public int startOrEnd;

    public Room(Vector2 pos, bool doorRoom, bool firstRoom) {
        this.pos = pos;
        if (firstRoom) {
            startOrEnd = 1;
            bool[] doors = {true, false, false, false};
            this.doors = doors;
        } else if (doorRoom) {
            startOrEnd = 2;
            bool[] doors = {false, false, true, false};
            this.doors = doors;
        } else {
            startOrEnd = 0;
            doors = new bool[4];
            setDoors();
        }
    }

    void setDoors() {
        for (int i = 0; i < 4; i++) {
            if (FloorGenerator.isRoomAtCoords(pos + modFromIndex(i))) {
                doors[i] = FloorGenerator.roomAtCoords(pos + modFromIndex(i)).doors[inverseIndex(i)];
            } else {
                doors[i] = Random.value <= .5f + indexRandMod(i);
                if (FloorGenerator.lastRound) {
                    doors[i] = false;
                }
            }
        }
    }

    float indexRandMod(int index) {
        switch (index)
        {
            case 0:
                return 0.2f;
            case 1:
                return -(pos.x / FloorGenerator.size);
            case 2:
                return -0.1f;
            case 3:
                return (pos.x / FloorGenerator.size);
        }
        return 0;
    }

    public void createNewRooms() {
        for (int i = 0; i < 4; i++) {
            if (!FloorGenerator.isRoomAtCoords(pos + modFromIndex(i)) && doors[i]) {
                FloorGenerator.floor.Add(new Room(pos + modFromIndex(i), false, false));
            }
        }
    }

    Vector2 modFromIndex(int index) {
        switch (index) {
            case 0:
                return new Vector2(0, 1);
            case 1:
                return new Vector2(1, 0);
            case 2:
                return new Vector2(0, -1);
            case 3:
                return new Vector2(-1, 0);
        }
        return Vector2.zero;
    }

    int inverseIndex(int index) {
        switch (index) {
            case 0:
                return 2;
            case 1:
                return 3;
            case 2:
                return 0;
            case 3:
                return 1;
        }
        return -1;
    }

    public override string ToString() {
        string result = "";
        result += "\tN: " + doors[0] + "\n\tE: " + doors[1] + "\n\tS: " + doors[2] + "\n\tW: " + doors[3];
        result += "\n\tCoords: " + pos + "\n";
        Object.Instantiate(FloorGenerator.visualizers[startOrEnd], pos, Quaternion.identity);
        return result;
    }
}
