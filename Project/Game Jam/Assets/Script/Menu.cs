using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {
    void Update() {
        if (FindObjectOfType<Transitions>().transitioning || FindObjectOfType<Transitions>().detransitioning) {
            GetComponent<Canvas>().sortingOrder = -1;
        } else {
            GetComponent<Canvas>().sortingOrder = 1;
        }
    }
    
    public void load(int c) {
        Transitions.Transition(c);
    }
}
