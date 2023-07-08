using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Transform t;
    float timeSinceSpawn;
    
    public void Update() {
        timeSinceSpawn += Time.deltaTime;
        sr.color = new Color(.1f, .75f, .13f, 
                             (Mathf.Cos(timeSinceSpawn * 2f) + 5/3f) * 0.375f);
    }

    public void resetTimer() {
        timeSinceSpawn = 0f;
    }

    public void Destroy() {
        Object.Destroy(gameObject);
    }
}