using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ColorManager : MonoBehaviour {
    [SerializeField] Tilemap[] world;

    void Start() {
        setColor();
    }
    
    [ContextMenu("Change Color")]
    public Color setColor() {
        int prim = Random.Range(0, 3);
        byte[] colors = {0, 0, 0};
        colors[prim] = 255;
        int sec = Random.Range(0, 2);
        if (sec >= prim) {
            sec++;
        }
        colors[sec] = (byte)Random.Range(214, 256);
        int ter = 0;
        while (ter == sec || ter == prim) {
            ter++;
        }
        colors[ter] = 213;
        Color c = new Color32(colors[0], colors[1], colors[2], 255);
        foreach (Tilemap t in world) {
            t.color = c;
            t.RefreshAllTiles();
        }
        Camera.main.backgroundColor = new Color(c.r * (44/255.0f), c.g * (44/255.0f), c.b * (44/255.0f), 1f);
        return c;
    }

    public Color setColor(Color c) {
        foreach (Tilemap t in world) {
            t.color = c;
            t.RefreshAllTiles();
        }
        Camera.main.backgroundColor = new Color(c.r * (44/255.0f), c.g * (44/255.0f), c.b * (44/255.0f), 1f);
        return c;
    }
}
