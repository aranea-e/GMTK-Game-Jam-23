using UnityEngine;

public class Item : MonoBehaviour {
    public WeaponSO w;
    public Transform t;
    public SpriteRenderer s;
    public bool invenSlot;

    void Awake() {
        if (!invenSlot) {
            s.sprite = w.s;
        }
    }
}
