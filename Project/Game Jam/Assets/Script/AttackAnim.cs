using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnim : MonoBehaviour
{
    bool ranged;
    bool player;
    Sprite sprite;
    Vector3 from;
    Vector3 to;
    [SerializeField] Transform t;
    [SerializeField] SpriteRenderer sr;
    float timer;
    float adTime;

    public void passVars(bool ranged, bool player, Sprite sprite, Vector3 from, Vector3 to) {
        this.ranged = ranged;
        this.player = player;
        this.sprite = sprite;
        this.from = from;
        this.to = to;
        timer = 0f;
        Update();
    }

    void Update() {
        sr.sprite = sprite;
        timer += Time.deltaTime;
        if (player) {
            adTime = timer;
        } else {
            adTime = timer - 0.5f;
        }

        if (adTime < 0f) {
            adTime = 0f;
            sr.enabled = false;
        } else if (adTime > 0.5f) {
            Destroy(gameObject);
        } else {
            sr.enabled = true;
        }

        if (ranged) {
            Util.moveSprite(t, from, to, adTime);
            Vector3 target = from - to;
            float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg + 90;
            t.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        } else {
            t.position = from + new Vector3(0.5f, 0.5f, 0);
            Vector3 target = from - to;
            float mod = Util.calculateMod(timer) * (0 - 60) + 30;
            float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg + 90 + mod;
            target = Quaternion.AngleAxis(angle - 90, Vector3.forward) * Vector3.right;
            t.position = Vector3.MoveTowards(t.position, t.position - target, 1f);
            t.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }
}
