using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionHandler : MonoBehaviour {
    public Option[] options;
    public OptionSO[] optionSOs;
    public int selected;

    void Awake() {
        Enemy.options = optionSOs;
    }

    void Update() {
        for (int i = 0; i < options.Length; i++) {
            if (Input.GetMouseButtonDown(0) && Util.mouseInBounds(options[i].bsr.bounds.min, options[i].bsr.bounds.max)) {
                selected = i;
            }
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

        foreach (Enemy e in FindObjectsOfType<Enemy>()) {
            if (Input.GetMouseButtonDown(0) && Util.mouseInBounds(e.t.position, e.t.position + new Vector3(1f, 1f, 0f))) {
                e.currentOption = optionSOs[selected];
            }
        }
    }
}
