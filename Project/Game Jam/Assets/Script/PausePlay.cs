using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePlay : MonoBehaviour {
    public Slider s;
    public Image[] images;
    public Sprite[] sprites;
    
    void Update() {
        if (FindObjectOfType<Transitions>().transitioning || FindObjectOfType<Transitions>().detransitioning) {
            GetComponent<Canvas>().sortingOrder = -1;
        } else {
            GetComponent<Canvas>().sortingOrder = 1;
        }
        FindObjectOfType<GameManagerHelper>().moveInterval = s.value;
        foreach (Image i in images) {
            i.enabled = GameManager.canPause;
        }
        if (GameManager.isPaused) {
            images[0].sprite = sprites[1];
        } else {
            images[0].sprite = sprites[0];
        }
    }

    public void buttonClicked() {
        if (GameManager.canPause){
            GameManager.isPaused = !GameManager.isPaused;
            GameManager.delay = 0f;
            GameManager.moveTimer = GameManager.moveInterval / 2;
        }
    }
}
