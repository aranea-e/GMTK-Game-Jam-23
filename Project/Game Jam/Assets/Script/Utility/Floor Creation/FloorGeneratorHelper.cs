using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorGeneratorHelper : MonoBehaviour {
    public GameObject[] visualizers;
    public int baseSize;
    public TemplateRoomArray[] templates;

    

    void Start() {
        FloorGenerator.visualizers = visualizers;
        FloorGenerator.createFloor(baseSize);
        Generate();
    }

    [ContextMenu("Debug")]
    void FGDebug() {
        Debug.Log(FloorGenerator.FGDebug());
    }

    void Generate() {
        for (int i = 0; i < FloorGenerator.floor.Count; i++) {
            string name = "";
            if (FloorGenerator.floor[i].doors[0]) { name += "North"; }
            if (FloorGenerator.floor[i].doors[1]) { name += "East"; }
            if (FloorGenerator.floor[i].doors[2]) { name += "South"; }
            if (FloorGenerator.floor[i].doors[3]) { name += "West"; }

            if (FloorGenerator.floor[i].startOrEnd == 2) { name = "Door"; }
            if (FloorGenerator.floor[i].startOrEnd == 1) { name = "Start"; }

            for (int j = 0; j < templates.Length; j++) {
                if (templates[j].name == name) {
                    FloorGenerator.floor[i].template = templates[j].fetchRoom();
                }
            }
        }
    }
}
