using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class GameManager {
    static TargetSO[] targets;
    static GameObject targetPrefab;
    public static List<Target> targetInstances;
    static Tilemap[] world;

    public static void setup(TargetSO[] t, GameObject tp, Tilemap[] tmps) {
        targets = t;
        targetPrefab = tp;
        targetInstances =  new List<Target>();
        world = tmps;
    }

    public static void start() {
        createMoveTargets();
    }

    public static void onPlayerAction() {
        destroyAllTargets();
        createMoveTargets();
    }

    static void createMoveTargets() {
        Vector3[] coords = {new Vector3(Util.getP().t.position.x + 1, Util.getP().t.position.y, 0),
                            new Vector3(Util.getP().t.position.x - 1, Util.getP().t.position.y, 0),
                            new Vector3(Util.getP().t.position.x, Util.getP().t.position.y + 1, 0),
                            new Vector3(Util.getP().t.position.x, Util.getP().t.position.y - 1, 0)};
        foreach (Vector3 v in coords) {
            if (!world[0].HasTile(Vector3Int.FloorToInt(v))) {
                instantiateTarget(targets[0], targetPrefab, v);
            }
        }
    }

    static void instantiateTarget(TargetSO so, GameObject p, Vector3 pos) {
        targetInstances.Add(Object.Instantiate(p, pos, Quaternion.identity).GetComponent<Target>());
        targetInstances[targetInstances.Count - 1].so = so;
    }

    static void destroyAllTargets() {
        foreach (Target t in targetInstances) {
            t.Destroy();
        }
        targetInstances.Clear();
    }
}
