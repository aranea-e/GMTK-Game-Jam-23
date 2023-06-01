using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
    public List<GameObject> slots;
    [SerializeField] GameObject slotPF;
    
    void Awake() {
        slots = new List<GameObject>();
    }

    void Update() {
        for (int i = 0; i < slots.Count; i++) {
            slots[i].transform.position = new Vector3(-(slots.Count - 1f) * 10f/16f + i * 20f/16f, -4f, 0f);
            Vector2 low = new Vector2(slots[i].transform.position.x - 0.5f, slots[i].transform.position.y - 0.5f);
            Vector2 high = new Vector2(slots[i].transform.position.x + 0.5f, slots[i].transform.position.y + 0.5f);
            if (Input.GetMouseButtonDown(0) && Util.mouseInBounds(low, high) && GameManager.timer >= 1) {
                if (Util.getP().w != slots[i].GetComponent<Weapon>().w) {
                    GameManager.onPlayerAction();
                    GameManager.timer += .5f;
                }
                Util.getP().w = slots[i].GetComponent<Weapon>().w;
            }
        }
    }

    public void addSlot(WeaponSO w) {
        slots.Add(Instantiate(slotPF, gameObject.transform.position, Quaternion.identity));
        slots[slots.Count - 1].transform.parent = gameObject.transform;
        slots[slots.Count - 1].GetComponent<Weapon>().w = w;
        slots[slots.Count - 1].GetComponent<Weapon>().s.sprite = w.s;
        Update();
    }
}
