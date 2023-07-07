using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class GameManager {
    static TargetSO[] targets;
    static GameObject targetPrefab;
    static GameObject attackPrefab;
    public static List<Target> targetInstances;
    public static Tilemap[] world;
    public static LayerMask wallLM;
    public static float timer;
    public static bool infiniteInven;
    public static WeaponSO[] baseWeaponSOs;
    public static WeaponSO[] weaponSOs;
    public static float moveInterval;
    public static float moveTimer;

    public static void setup(TargetSO[] t, GameObject tp, GameObject ap, Tilemap[] tmps, 
                             LayerMask lm, bool ii, WeaponSO[] bweapons, WeaponSO[] weapons,
                             float mI) {
        targets = t;
        targetPrefab = tp;
        attackPrefab = ap;
        world = tmps;
        wallLM = lm;
        infiniteInven = ii;
        baseWeaponSOs = bweapons;
        weaponSOs = weapons;
        moveInterval = mI;
        revertWeapons();
    }

    public static void revertWeapons() {
        for (int i = 0; i < baseWeaponSOs.Length; i++) {
            weaponSOs[i].atk = baseWeaponSOs[i].atk;
            weaponSOs[i].ran = baseWeaponSOs[i].ran;
        }
    }

    public static void start() {
        timer = 1f;
    }

    public static void onPlayerAction() {
        timer = 0f;
        if (Util.getP().testing) {
            return;
        }
        foreach (Enemy e in Object.FindObjectsOfType<Enemy>()) {
            e.pathfind();
            e.Update();
        }
    }

    public static void createMoveTargets() {
        destroyAllTargets();
        targetInstances = new List<Target>();
        Vector3[] coords = {new Vector3(Util.getP().t.position.x + 1, Util.getP().t.position.y, 0),
                            new Vector3(Util.getP().t.position.x - 1, Util.getP().t.position.y, 0),
                            new Vector3(Util.getP().t.position.x, Util.getP().t.position.y + 1, 0),
                            new Vector3(Util.getP().t.position.x, Util.getP().t.position.y - 1, 0)};
        foreach (Vector3 v in coords) {
            if (Util.tileAvailable(v) || Util.isDoor(v)) {
                instantiateTarget(targets[0], targetPrefab, v);
            }
        }

        foreach (Enemy e in Object.FindObjectsOfType<Enemy>()) {
            if (Util.lineOfSight(Util.getP().t.position, e.t.position)) {
                instantiateTarget(targets[1], targetPrefab, e.t.position);
            }
        }
    }

    #region enemy checks
    public static bool isEnemyInSight(float range) {
        foreach (Enemy e in Object.FindObjectsOfType<Enemy>()) {
            if (Util.lineOfSight(Util.getP().t.position, e.t.position)
                && Vector3.Distance(Util.getP().t.position, e.t.position) <= range) {
                return true;
            }
        }
        return false;
    }

    public static bool isEnemy(float range) {
        foreach (Enemy e in Object.FindObjectsOfType<Enemy>()) {
            if (Vector3.Distance(Util.getP().t.position, e.t.position) <= range) {
                return true;
            }
        }
        return false;
    }

    public static Enemy closestEnemyInSight() {
        Enemy result = null;
        float distance = -1f;
        foreach (Enemy e in Object.FindObjectsOfType<Enemy>()) {
            if (Util.lineOfSight(Util.getP().t.position, e.t.position)
                && (Vector3.Distance(Util.getP().t.position, e.t.position) < distance || distance == -1f)) {
                distance = Vector3.Distance(Util.getP().t.position, e.t.position);
                result = e;
            }
        }
        return result;
    }

    public static Enemy closestEnemy() {
        Enemy result = null;
        float distance = -1f;
        foreach (Enemy e in Object.FindObjectsOfType<Enemy>()) {
            if (Vector3.Distance(Util.getP().t.position, e.t.position) < distance || distance == -1f) {
                distance = Vector3.Distance(Util.getP().t.position, e.t.position);
                result = e;
            }
        }
        return result;
    }
    #endregion

    #region item checks
    public static bool isItemInSight(float range) {
        foreach (Item i in Object.FindObjectsOfType<Item>()) {
            if (Util.lineOfSight(Util.getP().t.position, i.t.position)
                && Vector3.Distance(Util.getP().t.position, i.t.position) <= range) {
                return true;
            }
        }
        return false;
    }

    public static bool isItem(float range) {
        foreach (Item i in Object.FindObjectsOfType<Item>()) {
            if (Vector3.Distance(Util.getP().t.position, i.t.position) <= range) {
                return true;
            }
        }
        return false;
    }

    public static Item closestItemInSight() {
        Item result = null;
        float distance = -1f;
        foreach (Item i in Object.FindObjectsOfType<Item>()) {
            if (Util.lineOfSight(Util.getP().t.position, i.t.position)
                && (Vector3.Distance(Util.getP().t.position, i.t.position) < distance || distance == -1f)) {
                distance = Vector3.Distance(Util.getP().t.position, i.t.position);
                result = i;
            }
        }
        return result;
    }

    public static Item closestItem() {
        Item result = null;
        float distance = -1f;
        foreach (Item i in Object.FindObjectsOfType<Item>()) {
            if (Vector3.Distance(Util.getP().t.position, i.t.position) < distance || distance == -1f) {
                distance = Vector3.Distance(Util.getP().t.position, i.t.position);
                result = i;
            }
        }
        return result;
    }
    #endregion

    static void instantiateTarget(TargetSO so, GameObject p, Vector3 pos) {
        targetInstances.Add(Object.Instantiate(p, pos, Quaternion.identity).GetComponent<Target>());
        targetInstances[targetInstances.Count - 1].so = so;
        targetInstances[targetInstances.Count - 1].Update(false);
    }

    public static void destroyAllTargets() {
        if (targetInstances == null) { return; }
        foreach (Target t in targetInstances) {
            t.Destroy();
        }
        targetInstances = null;
    }

    public static void attackAnim(bool ranged, bool player, Sprite sprite, Vector3 from, Vector3 to) {
        Object.Instantiate(attackPrefab, from, Quaternion.identity).GetComponent<AttackAnim>().passVars(
            ranged, player, sprite, from, to);
    }
}
