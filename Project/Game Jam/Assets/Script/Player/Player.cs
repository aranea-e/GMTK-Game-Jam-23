using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public Transform t;
    [SerializeField] Transform door;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Transform srt;
    [SerializeField] Sprite[] s;
    Vector3 last;
    public WeaponSO w;
    public int mhp;
    public int hp;
    [SerializeField] Transform hpbar;
    public Inventory inven;
    
    void Awake() {
        last = t.position;
        inven = FindObjectOfType<Inventory>();
        Util.startup(this, door);
    }

    [ContextMenu("stay")]
    void stay() {
        move(t.position);
    }

    public void Update() {
        GameManager.timer += Time.deltaTime;
        if (GameManager.timer >= 1f || (GameManager.timer >= .5f && !Util.enemiesMoving())) {
            GameManager.timer = 1f;
            hpbar.localScale = new Vector3((hp/(float) mhp) * (10/16f), (2/16f), 1);
            if (GameManager.targetInstances == null) {
                GameManager.createMoveTargets();
            }
        } else {
            GameManager.destroyAllTargets();
        }
        
        if (GameManager.timer < 0.5f) {
            Util.moveSprite(srt, last, t.position, GameManager.timer);
        } else {
            Util.moveSprite(srt, last, t.position, 0.5f);
        }
    }

    public void setPos(Vector2 v) {
        last = t.position;
        t.position = new Vector3(v.x, v.y, 0f);
    }

    public void move(Vector2 v) {
        if (Util.isDoor(v)) {
            Debug.Log("u win!!1!");
        }
        if (Util.itemOccupying(v) != null) {
            if (Util.itemOccupying(v).invenSlot) {
                inven.addSlot();
                Destroy(Util.itemOccupying(v).gameObject);
            } else if (inven.contains(Util.itemOccupying(v).w) != -1) {
                if (Util.itemOccupying(v).mods.x > 0 || Util.itemOccupying(v).mods.y > 0) {
                    Util.itemOccupying(v).w.atk += (int) Util.itemOccupying(v).mods.x;
                    Util.itemOccupying(v).w.ran += (int) Util.itemOccupying(v).mods.y;
                    inven.changeSlot(inven.contains(Util.itemOccupying(v).w), Util.itemOccupying(v).w);
                    Destroy(Util.itemOccupying(v).gameObject);
                }
            } else if (inven.isFull()) {
                if (GameManager.infiniteInven) {
                    Util.itemOccupying(v).w.atk += (int) Util.itemOccupying(v).mods.x;
                    Util.itemOccupying(v).w.ran += (int) Util.itemOccupying(v).mods.y;
                    inven.addSlot(Util.itemOccupying(v).w);
                    Destroy(Util.itemOccupying(v).gameObject);
                }
            } else {
                Util.itemOccupying(v).w.atk += (int) Util.itemOccupying(v).mods.x;
                Util.itemOccupying(v).w.ran += (int) Util.itemOccupying(v).mods.y;
                inven.changeSlot(Util.itemOccupying(v).w);
                Destroy(Util.itemOccupying(v).gameObject);
            }
        }
        Util.face(t.position, v, sr, s);
        setPos(v);
        Util.moveSprite(srt, last, t.position, 0f);
    }

    public void attack(Vector2 v) {
        foreach (Enemy e in FindObjectsOfType<Enemy>()) {
            if (((Vector2) e.t.position) == v) {
                e.damage(w.atk);
            }
        }
        Util.face(t.position, v, sr, s);
        last = t.position;
        GameManager.attackAnim((w.ran > 1), true, w.s, t.position, v);
    }

    public void damage(int damage) {
        hp -= damage;
        if (hp <= 0) {
            Debug.Log("L bozo");
            hp = 0;
        }
    }
}
