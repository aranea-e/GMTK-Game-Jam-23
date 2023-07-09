using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class GameManager {
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
    public static bool isPaused;
    public static bool canPause;
    public static float delay;
    public static bool showAnims;

    public static void setup(GameObject tp, GameObject ap, Tilemap[] tmps, 
                             LayerMask lm, bool ii, WeaponSO[] bweapons, WeaponSO[] weapons,
                             float mI) {
        targetInstances = new List<Target>();
        targetPrefab = tp;
        attackPrefab = ap;
        world = tmps;
        wallLM = lm;
        infiniteInven = ii;
        baseWeaponSOs = bweapons;
        weaponSOs = weapons;
        moveInterval = mI;
        canPause = false;
        isPaused = true;
    }

    public static void revertWeapons() {
        if (baseWeaponSOs == null || weaponSOs == null) {
            return;
        }
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

    public static void instantiateTarget(Vector3 pos) {
        targetInstances.Add(Object.Instantiate(targetPrefab, pos, Quaternion.identity).GetComponent<Target>());
        for (int i = 0; i < targetInstances.Count; i++) {
            targetInstances[i].resetTimer();
            targetInstances[i].Update();
        }
    }

    public static void destroyAllTargets() {
        if (targetInstances == null) { return; }
        foreach (Target t in targetInstances) {
            t.Destroy();
        }
        targetInstances = new List<Target>();
    }

    public static void attackAnim(bool ranged, bool player, Sprite sprite, Vector3 from, Vector3 to) {
        timer = 0f;
        Object.Instantiate(attackPrefab, from, Quaternion.identity).GetComponent<AttackAnim>().passVars(
            ranged, player, sprite, from, to);
    }
}
