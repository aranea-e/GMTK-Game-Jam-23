using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
    public List<GameObject> slots;
    [SerializeField] GameObject slotPF;
    [SerializeField] WeaponSO fistSO;

    public void updateInven() {
        if (GameManager.isEnemyInSight(7)) {
            Util.getP().w = chooseWeapon(GameManager.closestEnemyInSight().so.def);
        }
    }

    public WeaponSO chooseWeapon(int def) {
        List<WeaponSO> available = new List<WeaponSO>();
        for (int i = 0; i < slots.Count; i++) {
            if (Pathfind.isInRange(Util.getP().t.position, GameManager.closestEnemyInSight().t.position, slots[i].GetComponent<Weapon>().w.ran) 
                && slots[i].GetComponent<Weapon>().w.atk > def){
                available.Add(slots[i].GetComponent<Weapon>().w);
            }
        }

        if (available.Count == 0) { return fistSO; }

        int result = 0;
        for (int i = 0; i < available.Count; i++) {
            if (available[i].atk > available[result].atk) {
                result = i;
            }
        }
        return available[result];
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
    }

    [ContextMenu("add slot")]
    public void addSlot() {
        slots.Add(Instantiate(slotPF, gameObject.transform.position, Quaternion.identity));
        slots[slots.Count - 1].transform.parent = gameObject.transform;
        slots[slots.Count - 1].GetComponent<Weapon>().w = null;
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
