using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Transitions : MonoBehaviour{
    public GameObject transitionBlock;
    public GameObject[][] blocks;
    public GameObject blockParent;
    public Vector2[][] blockTargets;
    public Vector2[][] blockInit;
    public float time;
    public bool transitioning;
    public bool detransitioning;
    public TextMeshProUGUI text;
    public Vector2 textInit;
    public static int floor;
    public int con;

    void Awake() {
        if (Object.FindObjectsOfType<Transitions>().Length > 1) {
            Destroy(gameObject);
        } else {
            DontDestroyOnLoad(gameObject);
            floor = 0;
        }
    }

    void Update() {
        if (transitioning) {
            time += Time.deltaTime;
            for (int x = 0; x < blocks.Length; x++) {
                for (int y = 0; y < blocks[x].Length; y++) {
                    float delay = (((17 - x) + (40 - y * 4)) / 57f);
                    float adjusted = time - delay;
                    if (adjusted < 0f) {
                        adjusted = 0f;
                    }
                    if (adjusted > 0.5f) {
                        adjusted = 0.5f;
                    }
                    float tPercent = adjusted / 0.5f;
                    float iPercent = (0.5f - adjusted) / 0.5f;
                    Vector2 target = blockInit[x][y] * iPercent + blockTargets[x][y] * tPercent;
                    blocks[x][y].GetComponent<RectTransform>().anchoredPosition = target;
                }
            }
            text.text = "";
            if (time > 1f) {
                switch (con) {
                    case 1:
                        text.text = "Game Over";
                        break;
                    case 2:
                        text.text = "Level " + floor.ToString();
                        break;
                    case 3:
                        text.text = "Dungeon defence success! Thanks for playing!";
                        break;
                }
                float adjusted = time - 1f;
                if (adjusted > 0.5f) {
                    adjusted = 0.5f;
                }
                float percent = (0.5f - adjusted) / 0.5f;
                text.GetComponent<RectTransform>().anchoredPosition = textInit * percent;
            }
            if (time > 2f) {
                transitioning = false;
                detransitioning = true;
                switch (con) {
                    case 3:
                    case 1:
                        SceneManager.LoadScene(0);
                        break;
                    case 2:
                        SceneManager.LoadScene(1);
                        break;
                }
            }
        }
        if (detransitioning) {
            time -= Time.deltaTime;
            for (int x = 0; x < blocks.Length; x++) {
            for (int y = 0; y < blocks[x].Length; y++) {
                float delay = (((17 - x) + (40 - y * 4)) / 57f);
                float adjusted = time - delay;
                if (adjusted < 0f) {
                    adjusted = 0f;
                }
                if (adjusted > 0.5f) {
                    adjusted = 0.5f;
                }
                float tPercent = adjusted / 0.5f;
                float iPercent = (0.5f - adjusted) / 0.5f;
                Vector2 target = blockInit[x][y] * iPercent + blockTargets[x][y] * tPercent;
                blocks[x][y].GetComponent<RectTransform>().anchoredPosition = target;
            }
            }
            text.text = "";
            if (time > 1f) {
                switch (con) {
                    case 1:
                        text.text = "Game Over";
                        break;
                    case 2:
                        text.text = "Level " + floor.ToString();
                        break;
                    case 3:
                        text.text = "Dungeon defence success! Thanks for playing!";
                        break;
                }
                float adjusted = time - 1f;
                if (adjusted > 0.5f) {
                    adjusted = 0.5f;
                }
                float percent = (0.5f - adjusted) / 0.5f;
                text.GetComponent<RectTransform>().anchoredPosition = textInit * percent;
            }
            if (time <= 0f) {
                time = 0f;
                detransitioning = false;
                foreach (GameObject[] arr in blocks)
                {
                    foreach (GameObject block in arr) {
                        Destroy(block);
                    }
                }
                GameManager.isPaused = false;
            }
        }
    }

    public static void Transition(int c) {
        Object.FindObjectOfType<Transitions>().trans(c);
    }

    void trans(int c) {
        this.con = c;
        switch (con) {
            case 1:
                floor = 0;
                break;
            case 2:
                floor++;
                if (floor > 4)
                {
                    trans(3);
                    return;
                }
                break;
            case 3:
                floor = 0;
                break;
        }
        time = 0f;
        transitioning = true;
        blocks = new GameObject[17][];
        blockTargets = new Vector2[17][];
        blockInit = new Vector2[17][];
        for (int i = 0; i < 17; i++) {
            blocks[i] = new GameObject[10];
            blockTargets[i] = new Vector2[10];
            blockInit[i] = new Vector2[10];
        }
        for (int x = 0; x < blocks.Length; x++) {
            for (int y = 0; y < blocks[x].Length; y++) {
                blocks[x][y] = Instantiate(transitionBlock, Vector3.zero, Quaternion.identity, blockParent.transform);
                blockInit[x][y] = new Vector2(50 * x, -100f);
                blockTargets[x][y] = new Vector2(50 * x, 50 * y);
                blocks[x][y].GetComponent<RectTransform>().anchoredPosition = blockInit[x][y];
            }
        }
    }

    [ContextMenu("test")]
    public void test() {
        trans(1);
    }
}
