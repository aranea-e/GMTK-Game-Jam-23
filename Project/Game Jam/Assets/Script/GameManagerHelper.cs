using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManagerHelper : MonoBehaviour {
    [SerializeField] TargetSO[] targets;
    [SerializeField] GameObject targetPrefab;
    [SerializeField] Tilemap[] world;

    void Awake() {
        GameManager.setup(targets, targetPrefab, world);
    }

    void Start() {
        GameManager.start();
        Destroy(gameObject);
    }
}
