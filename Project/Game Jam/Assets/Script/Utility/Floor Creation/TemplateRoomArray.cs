using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TemplateRoomArray {
    public string name;
    public GameObject[] rooms;

    public GameObject fetchRoom() {
        return rooms[Random.Range(0, rooms.Length)];
    }
}
