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
    Weapon weapon;
    int type;


    void Update() {
        rt.anchoredPosition = Util.mousePosition();
        if (c != null){
                Destroy(c.gameObject);
        }
        bool found = false;
        if (Util.itemOccupying(Util.mousePositionInt()) != null && !Util.itemOccupying(Util.mousePositionInt()).invenSlot) {
            c = Instantiate(co[0], rt).GetComponent<Card>();
            item = Util.itemOccupying(Util.mousePositionInt());
            type = 0;
            if (Util.mousePosition().y - Util.screenBottom() < 3f/2f) {
                rt.anchoredPosition = new Vector2(Util.mousePosition().x, Util.mousePosition().y + 3f/2f);
            }
            found = true;
        }
        if (Util.creatureOccupying(Util.mousePositionInt())) {
            if (c != null){
                Destroy(c.gameObject);
            }
            c = Instantiate(co[1], rt).GetComponent<Card>();
            enemy = Util.creatureAt(Util.mousePositionInt());
            type = 1;
            if (Util.mousePosition().y - Util.screenBottom() < 41f/16f) {
                rt.anchoredPosition = new Vector2(Util.mousePosition().x, Util.mousePosition().y + 41f/16f);
            }
            found = true;
        }
        

        if (found) {
            switch (type) {
                case 0:
                    setPins(c.atk, item.w.atk + (int) item.mods.x);
                    setPins(c.ran, item.w.ran + (int) item.mods.y);
                    break;

                case 1:
                    setPins(c.hp, enemy.hp, enemy.so.hp, new Color(1f, 0f, 0f, 1f), new Color(.25f, 0f, 0f, 1f));
                    setPins(c.def, enemy.so.def);
                    setPins(c.spd, enemy.so.spd);
                    setPins(c.vran, enemy.so.vran);
                    setPins(c.atk, enemy.so.atk);
                    setPins(c.ran, enemy.so.ran);
                    break;

                case 2:
                    setPins(c.atk, weapon.w.atk);
                    setPins(c.ran, weapon.w.ran);
                    break;
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
