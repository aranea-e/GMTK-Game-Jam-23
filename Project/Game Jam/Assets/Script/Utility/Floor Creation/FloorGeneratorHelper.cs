using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FloorGeneratorHelper : MonoBehaviour {
    public GameObject[] visualizers;
    public int baseSize;
    public TemplateRoomArray[] templates;
    public Tilemap[] tmps;
    

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
                    placeRoom(FloorGenerator.floor[i].pos, FloorGenerator.floor[i].template);
                }
            }
        }
    }

    void placeRoom(Vector2 coords, GameObject template) {
        template = Instantiate(template);
        
        Tilemap enemySpawns = template.GetComponent<TemplateTilemapHandler>().tmps[0];
        Tilemap equipSpawns = template.GetComponent<TemplateTilemapHandler>().tmps[1];
        Tilemap walls = template.GetComponent<TemplateTilemapHandler>().tmps[2];
        Tilemap floor = template.GetComponent<TemplateTilemapHandler>().tmps[3];

        walls.CompressBounds();
        Vector2 templateOrigin = walls.localBounds.min;
        Vector2 origin = coords * 7;
        origin = new Vector2(origin.x - 3, origin.y - 3);

        for (int x = 0; x < 7; x++) {
            for (int y = 0; y < 7; y++) {
                tmps[0].SetTile((new Vector3Int(x + (int) origin.x, y + (int) origin.y, 0)), 
                    walls.GetTile(new Vector3Int(x + (int) templateOrigin.x, y + (int) templateOrigin.y, 0)));
            }
        }

        for (int x = 0; x < 7; x++) {
            for (int y = 0; y < 7; y++) {
                tmps[1].SetTile((new Vector3Int(x + (int) origin.x, y + (int) origin.y, 0)), 
                    floor.GetTile(new Vector3Int(x + (int) templateOrigin.x, y + (int) templateOrigin.y, 0)));
            }
        }

        Destroy(template);
    }
}
