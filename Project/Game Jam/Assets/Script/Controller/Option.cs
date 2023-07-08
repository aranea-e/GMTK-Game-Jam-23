using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Option : MonoBehaviour {
    public OptionSO so;
    public SpriteRenderer bsr;

    void Update() {
        GetComponent<SpriteRenderer>().sprite = so.s;
    }
}
