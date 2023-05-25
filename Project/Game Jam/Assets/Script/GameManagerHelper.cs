using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManagerHelper : MonoBehaviour {
    [SerializeField] TargetSO[] targets;
    [SerializeField] GameObject targetPrefab;
    [SerializeField] Tilemap[] world;
    [SerializeField] LayerMask lm;

    void Awake() {
        GameManager.setup(targets, targetPrefab, world, lm);
    }

    void Start() {
        GameManager.start();
        Destroy(gameObject);
    }
}
