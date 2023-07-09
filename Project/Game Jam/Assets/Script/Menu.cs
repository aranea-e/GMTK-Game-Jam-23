using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour {
    public Button b;
    public Button t;
    public Slider m;
    public Slider sfx;

    void Update() {
        if (FindObjectOfType<Transitions>().transitioning || FindObjectOfType<Transitions>().detransitioning) {
            GetComponent<Canvas>().sortingOrder = -1;
            b.enabled = false;
            t.enabled = false;
            m.enabled = false;
            sfx.enabled = false;
        } else {
            GetComponent<Canvas>().sortingOrder = 1;
            b.enabled = true;
            t.enabled = true;
            m.enabled = true;
            sfx.enabled = true;
        }
        FindObjectOfType<audioManager>().changeVolume(sfx.value);
        FindObjectOfType<audioManager>().changeVolume("music", m.value);
    }
    
    public void load(int c) {
        Transitions.Transition(c);
    }
}
