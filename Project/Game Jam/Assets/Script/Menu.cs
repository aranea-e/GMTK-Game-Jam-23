using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour {
    public Button b;

    void Update() {
        if (FindObjectOfType<Transitions>().transitioning || FindObjectOfType<Transitions>().detransitioning) {
            GetComponent<Canvas>().sortingOrder = -1;
            b.enabled = false;
        } else {
            GetComponent<Canvas>().sortingOrder = 1;
            b.enabled = true;
        }
    }
    
    public void load(int c) {
        Transitions.Transition(c);
    }
}
