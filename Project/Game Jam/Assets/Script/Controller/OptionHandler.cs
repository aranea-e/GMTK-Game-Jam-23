using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionHandler : MonoBehaviour {
    public Option[] options;
    public OptionSO[] optionSOs;
    public int selected;
    public bool canGive;
    bool heal;
    List<Vector2> moveTargets;
    public int numTargets;
    [SerializeField] SpriteRenderer nonInteractionField;

    void Awake() {
        Enemy.options = optionSOs;
        moveTargets = new List<Vector2>();
    }

    void Update() {
        bool newSelection = false;
        for (int i = 0; i < options.Length; i++) {
            if (Input.GetMouseButtonDown(0) && Util.mouseInBounds(options[i].bsr.bounds.min, options[i].bsr.bounds.max)) {
                if (selected != i && selected != -1) {
                    FindObjectOfType<audioManager>().play("select");
                }
                selected = i;
                newSelection = true;
                GameManager.destroyAllTargets();
                moveTargets = new List<Vector2>();
                numTargets = 0;
            }
        }   
        if (!newSelection && (selected == 2 || selected == 3) && Util.tileAvailable(Util.mousePositionInt()) 
            && Input.GetMouseButtonDown(0) && !targetExists(Util.mousePositionInt())) {
            if (selected == 2) {
                GameManager.destroyAllTargets();
                moveTargets = new List<Vector2>();
            }
            GameManager.instantiateTarget(Util.mousePositionInt());
            moveTargets.Add(Util.mousePositionInt());
            numTargets = moveTargets.Count;
            FindObjectOfType<audioManager>().play("select");
        }

        for (int i = 0; i < options.Length; i++) {
            if (i == selected) {
                options[i].bsr.color = new Color(1f, 1f, .8f, .85f);
                options[i].transform.localPosition = new Vector3(options[i].transform.localPosition.x, -5.75f, options[i].transform.localPosition.z);
            } else {
                options[i].bsr.color = new Color(1f, 1f, 1f, .5f);
                options[i].transform.localPosition = new Vector3(options[i].transform.localPosition.x, -6f, options[i].transform.localPosition.z);
            }
        }

        switch (selected) {
            case 0:
            case 1:
                canGive = true;
                heal = false;
                break;

            case 2:
            case 3:
                canGive = numTargets > selected - 2;
                heal = false;
                break;

            case 4:
                canGive = false;
                heal = true;
                break;

            default:
                canGive = false;
                heal = false;
                break;
        }

        if (!newSelection && !Util.mouseInBounds(nonInteractionField.bounds.min, nonInteractionField.bounds.max)) {
            foreach (Enemy e in FindObjectsOfType<Enemy>()) {
                if (Input.GetMouseButtonDown(0) && Util.mouseInBounds(e.sr.transform.position - new Vector3(0.5f, 0.5f, 0f), 
                                                                    e.sr.transform.position + new Vector3(0.5f, 0.5f, 0f))
                                                && FindObjectOfType<Treasury>().coins >= optionSOs[selected].price) {
                    if (canGive) {
                        e.currentOption = optionSOs[selected];
                        e.moveTargets = moveTargets;
                        GameManager.destroyAllTargets();
                        moveTargets = new List<Vector2>();
                        numTargets = 0;
                        FindObjectOfType<Treasury>().addCoin(-optionSOs[selected].price);
                        FindObjectOfType<audioManager>().play("select");
                    }
                    if (heal && e.hp < e.so.hp) {
                        e.heal();
                        FindObjectOfType<Treasury>().addCoin(-optionSOs[selected].price);
                        FindObjectOfType<audioManager>().play("select");
                    }
                }
            }
        }
    }

    bool targetExists(Vector2 t) {
        for (int i = 0; i < moveTargets.Count; i++){
            if (t == moveTargets[i]) {
                return true;
            }
        }
        return false;
    }
}
