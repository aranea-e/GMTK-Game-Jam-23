using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardCanvas : MonoBehaviour {
    Card c;
    [SerializeField] GameObject[] co;
    [SerializeField] RectTransform rt;
    Item item;
    Enemy enemy;
    bool isItem;


    void Update() {
        rt.anchoredPosition = Util.mousePosition();
        if (c != null){
                Destroy(c.gameObject);
        }
        bool found = false;
        if (Util.itemOccupying(Util.mousePositionInt()) != null && !Util.itemOccupying(Util.mousePositionInt()).invenSlot) {
            c = Instantiate(co[0], rt).GetComponent<Card>();
            item = Util.itemOccupying(Util.mousePositionInt());
            isItem = true;
            found = true;
        }
        if (Util.creatureOccupying(Util.mousePositionInt())) {
            if (c != null){
                Destroy(c.gameObject);
            }
            c = Instantiate(co[1], rt).GetComponent<Card>();
            enemy = Util.creatureAt(Util.mousePositionInt());
            isItem = false;
            found = true;
        }
        if (found) {
            if (isItem) {
                setPins(c.atk, item.w.atk);
                setPins(c.ran, item.w.ran);
            } else {
                setPins(c.hp, enemy.hp, enemy.so.hp, new Color(1f, 0f, 0f, 1f), new Color(.25f, 0f, 0f, 1f));
                setPins(c.def, enemy.so.def);
                setPins(c.spd, enemy.so.spd);
                setPins(c.vran, enemy.so.vran);
                setPins(c.atk, enemy.so.atk);
                setPins(c.ran, enemy.so.ran);
            }
        }
    }

    void setPins(Image[] pins, int num) {
        for (int i = 0; i < pins.Length; i++) {
            if (i < num) {
                pins[i].enabled = true;
            } else {
                pins[i].enabled = false;
            }
        }
    }

    void setPins(Image[] pins, int num, int num2, Color c1, Color c2) {
        for (int i = 0; i < pins.Length; i++) {
            if (i < num) {
                pins[i].enabled = true;
                pins[i].color = c1;
            } else if (i < num2) {
                pins[i].enabled = true;
                pins[i].color = c2;
            } else {
                pins[i].enabled = false;
            }
        }
    }
}
