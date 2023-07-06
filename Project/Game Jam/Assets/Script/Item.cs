using UnityEngine;

public class Item : MonoBehaviour {
    public WeaponSO w;
    public Transform t;
    public SpriteRenderer s;
    public bool invenSlot;
    public Vector2 mods;
    public Sprite iSprite;

    void Awake() {
        if (!invenSlot) {
            s.sprite = w.s;
        } else {
            s.sprite = iSprite;
        }
    }

    public void setup(WeaponSO w, Vector2 mods, bool invenSlot) {
        this.w = w;
        this.mods = mods;
        this.invenSlot = invenSlot;
        Awake();
    }
}
