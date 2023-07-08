using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManagerHelper : MonoBehaviour {
    [SerializeField] GameObject targetPrefab;
    [SerializeField] GameObject attackPrefab;
    [SerializeField] Tilemap[] world;
    [SerializeField] LayerMask lm;
    [SerializeField] bool ii;
    [SerializeField] WeaponSO[] bweapons;
    [SerializeField] WeaponSO[] weapons;
    [SerializeField] float moveInterval;

    void Awake() {
        GameManager.setup(targetPrefab, attackPrefab, world, lm, ii, bweapons, weapons, moveInterval);
    }

    void Start() {
        GameManager.start();
    }

    void Update() {
        GameManager.moveInterval = moveInterval;
    }
}
