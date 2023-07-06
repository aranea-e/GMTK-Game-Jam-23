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
            slots[i].transform.position = new Vector3(-(slots.Count - 1f) * 10f/16f + i * 20f/16f + transform.position.x, 
                                                        transform.position.y, 0f);
            Vector2 low = new Vector2(slots[i].transform.position.x - 0.5f, slots[i].transform.position.y - 0.5f);
            Vector2 high = new Vector2(slots[i].transform.position.x + 0.5f, slots[i].transform.position.y + 0.5f);
            if (Input.GetMouseButtonDown(0) && Util.mouseInBounds(low, high)) {
                Util.getP().w = slots[i].GetComponent<Weapon>().w;
                GameManager.createMoveTargets();
            }
            if (slots[i].GetComponent<Weapon>().w != null && Util.getP().w == slots[i].GetComponent<Weapon>().w) {
                slots[i].transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, .8f, .65f);
            } else {
                slots[i].transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, .5f);
            }
        }
    }

    public Weapon slotAt(Vector2 v) {
        for (int i = 0; i < slots.Count; i++) {
            slots[i].transform.position = new Vector3(-(slots.Count - 1f) * 10f/16f + i * 20f/16f + transform.position.x, 
                                                        transform.position.y, 0f);
            Vector2 low = new Vector2(slots[i].transform.position.x - 0.5f, slots[i].transform.position.y - 0.5f);
            Vector2 high = new Vector2(slots[i].transform.position.x + 0.5f, slots[i].transform.position.y + 0.5f);
            if (Util.mouseInBounds(low, high)) {
                return slots[i].GetComponent<Weapon>();
            }
        }
        return null;
    }

    public void addSlot(WeaponSO w) {
        slots.Add(Instantiate(slotPF, gameObject.transform.position, Quaternion.identity));
        slots[slots.Count - 1].transform.parent = gameObject.transform;
        slots[slots.Count - 1].GetComponent<Weapon>().w = w;
        slots[slots.Count - 1].GetComponent<Weapon>().s.sprite = w.s;
        Update();
    }

    [ContextMenu("add slot")]
    public void addSlot() {
        slots.Add(Instantiate(slotPF, gameObject.transform.position, Quaternion.identity));
        slots[slots.Count - 1].transform.parent = gameObject.transform;
        slots[slots.Count - 1].GetComponent<Weapon>().w = null;
        Update();
    }

    public void changeSlot(int index, WeaponSO w) {
        slots[index].GetComponent<Weapon>().w = w;
        slots[index].GetComponent<Weapon>().s.sprite = w.s;
    }

    public bool changeSlot(WeaponSO w) {
        foreach (GameObject slot in slots) {
            if (slot.GetComponent<Weapon>().w == null) {
                slot.GetComponent<Weapon>().w = w;
                slot.GetComponent<Weapon>().s.sprite = w.s;
                Update();
                return true;
            }
        }
        return false;
    }

    public bool isFull() {
        foreach (GameObject slot in slots) {
            if (slot.GetComponent<Weapon>().w == null) {
                return false;
            }
        }
        return true;
    }

    public int contains(WeaponSO w) {
        for (int i = 0; i < slots.Count; i++) {
            if (slots[i].GetComponent<Weapon>().w == w) {
                return i;
            }
        }
        return -1;
    }
}
