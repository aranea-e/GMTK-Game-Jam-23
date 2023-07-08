using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Treasury : MonoBehaviour {
    public GameObject coinPrefab;
    public Vector3 coinSpawn;
    public List<GameObject> coinObjects;
    public int coins;
    public TextMeshProUGUI number;

    void Awake() {
        coinObjects = new List<GameObject>();
    }

    void Update() {
        number.text = coins.ToString();
        while (coinObjects.Count < coins) {
            spawnCoin();
        }

        while (coinObjects.Count > coins) {
            destroyCoin();
        }

        for (int i = 0; i < coinObjects.Count; i++) {
            Vector3 pos = coinObjects[i].transform.localPosition;
            if (coinObjects[i].transform.localPosition.x < -0.85f) {
                pos.x = -0.85f;
            }
            if (coinObjects[i].transform.localPosition.x > 0.85f) {
                pos.x = 0.85f;
            }
            if (coinObjects[i].transform.localPosition.y < 0.65f) {
                pos.y = 0.65f;
            }
            coinObjects[i].transform.localPosition = pos;
        }
    }

    [ContextMenu("Add Coin")]
    public void addCoin() {
        coins++;
        Update();
    }

    public void addCoin(int coinsToAdd) {
        coins += coinsToAdd;
        Update();
    }

    void spawnCoin() {
        coinObjects.Add(Instantiate(coinPrefab, coinSpawn, Quaternion.identity));
        coinObjects[coinObjects.Count - 1].GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-25f, 25f), Random.Range(10f, 30f)));
        coinObjects[coinObjects.Count - 1].transform.SetParent(transform);
    }

    void destroyCoin() {
        Destroy(coinObjects[coinObjects.Count - 1]);
        coinObjects.RemoveAt(coinObjects.Count - 1);
    }
}
