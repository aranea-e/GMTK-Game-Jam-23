using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FloorGeneratorHelper : MonoBehaviour {
    public GameObject[] visualizers;
    public int baseSize;
    public int roomNumber;
    public int roomCount;
    public TemplateRoomArray[] templates;
    public Tilemap[] tmps;

    public GameObject[] enemies;
    public Sprite[] enemySprites;
    public WeaponSO[] items;
    public Sprite[] itemSprites;

    public GameObject weaponPrefab;
    public GameObject door;

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
        roomCount = FloorGenerator.floor.Count;
        for (int i = 0; i < FloorGenerator.floor.Count; i++) {
            roomNumber = i;
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
                    placeRoom(FloorGenerator.floor[i].pos, FloorGenerator.floor[i].template, 
                              FloorGenerator.floor[i].startOrEnd == 1, FloorGenerator.floor[i].startOrEnd == 2);
                }
            }
        }
    }

    void placeRoom(Vector2 coords, GameObject template, bool isStart, bool isDoor) {
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

        enemySpawns.RefreshAllTiles();
        for (int x = 0; x < 7; x++) {
            for (int y = 0; y < 7; y++) {
                if (enemySpawns.GetTile(new Vector3Int(x + (int) templateOrigin.x, y + (int) templateOrigin.y, 0)) != null &&
                    enemySpawns.GetTile<Tile>(new Vector3Int(x + (int) templateOrigin.x, y + (int) templateOrigin.y, 0)).sprite != null ) {
                    GameObject e = chooseEnemy(enemySpawns.GetTile<Tile>(new Vector3Int(x + (int) templateOrigin.x, y + (int) templateOrigin.y, 0)));
                    Instantiate(e, new Vector2(x + origin.x, y + origin.y), Quaternion.identity).GetComponent<Enemy>().setFGHVals(baseSize, roomNumber, roomCount);
                }
            }
        }

        equipSpawns.RefreshAllTiles();
        for (int x = 0; x < 7; x++) {
            for (int y = 0; y < 7; y++) {
                if (equipSpawns.GetTile(new Vector3Int(x + (int) templateOrigin.x, y + (int) templateOrigin.y, 0)) != null &&
                    equipSpawns.GetTile<Tile>(new Vector3Int(x + (int) templateOrigin.x, y + (int) templateOrigin.y, 0)).sprite != null ) {
                    WeaponSO w = chooseWeapon(equipSpawns.GetTile<Tile>(new Vector3Int(x + (int) templateOrigin.x, y + (int) templateOrigin.y, 0)));
                    Vector2 mods = new Vector2((Random.Range(0, 5) / 2), (Random.Range(0, 5) / 2));
                    if (w.ran == 1) { mods.y = 0f; }
                    if (isStart) { mods.x = 0f; mods.y = 0f; }
                    Instantiate(weaponPrefab, new Vector2(x + origin.x, y + origin.y), Quaternion.identity).GetComponent<Item>().setup(w, mods, false);
                }
            }
        }

        if (isDoor) {
            door.transform.position = new Vector3(origin.x + 3, origin.y + 3);
        }

        Destroy(template);
    }

    GameObject chooseEnemy(Tile t) {
        for (int i = 0; i < enemies.Length; i++) {
            if (enemySprites[i] == t.sprite) {
                return enemies[i];
            }
        }
        
        float j = 1f;
        foreach (GameObject enemy in enemies) {
            if (RandPercent(((j*j)/(j*j+1f))*100f)) {
                return enemy;
            }
            j += 2.5f;
        }
        return enemies[enemies.Length - 1];
    }

    WeaponSO chooseWeapon(Tile t) {
        for (int i = 0; i < items.Length; i++) {
            if (itemSprites[i] == t.sprite) {
                return items[i];
            }
        }
        
        foreach (WeaponSO weapon in items) {
            if (RandPercent(5f)) {
                return weapon;
            }
        }
        return items[items.Length - 1];
    }

    bool RandPercent(float percent) {
        return Random.value <= (percent / 100f);
    }
}
