using UnityEngine;

public class Item : MonoBehaviour {
    public WeaponSO w;
    public Transform t;
    public SpriteRenderer s;

    void Awake() {
        s.sprite = w.s;
    }
}
