using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Option : MonoBehaviour {
    public OptionSO so;

    void Update() {
        GetComponent<SpriteRenderer>().sprite = so.s;
    }
}
