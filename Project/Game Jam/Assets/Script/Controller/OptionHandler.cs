using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionHandler : MonoBehaviour {
    public Option[] options;
    public OptionSO[] optionSOs;
    public int selected;
    bool canGive;
    bool heal;
    List<Vector2> moveTargets;

    void Awake() {
        Enemy.options = optionSOs;
        moveTargets = new List<Vector2>();
    }

    void Update() {
        bool newSelection = false;
        for (int i = 0; i < options.Length; i++) {
            if (Input.GetMouseButtonDown(0) && Util.mouseInBounds(options[i].bsr.bounds.min, options[i].bsr.bounds.max)) {
                selected = i;
                newSelection = true;
                GameManager.destroyAllTargets();
                moveTargets = new List<Vector2>();
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
                canGive = moveTargets.Count > 0;
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

        if (!newSelection) {
            foreach (Enemy e in FindObjectsOfType<Enemy>()) {
                if (Input.GetMouseButtonDown(0) && Util.mouseInBounds(e.sr.transform.position - new Vector3(0.5f, 0.5f, 0f), 
                                                                    e.sr.transform.position + new Vector3(0.5f, 0.5f, 0f))) {
                    if (canGive) {
                        e.currentOption = optionSOs[selected];
                        e.moveTargets = moveTargets;
                        GameManager.destroyAllTargets();
                        moveTargets = new List<Vector2>();
                    }
                    if (heal) {
                        e.heal();
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
