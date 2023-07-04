using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManagerHelper : MonoBehaviour {
    [SerializeField] TargetSO[] targets;
    [SerializeField] GameObject targetPrefab;
    [SerializeField] GameObject attackPrefab;
    [SerializeField] Tilemap[] world;
    [SerializeField] LayerMask lm;
    [SerializeField] bool ii;
    [SerializeField] WeaponSO[] bweapons;
    [SerializeField] WeaponSO[] weapons;

    void Awake() {
        GameManager.setup(targets, targetPrefab, attackPrefab, world, lm, ii, bweapons, weapons);
    }

    void Start() {
        GameManager.start();
        Destroy(gameObject);
    }
}
