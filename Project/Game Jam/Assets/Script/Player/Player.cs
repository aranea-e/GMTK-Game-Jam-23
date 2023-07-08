using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public string target;
    public Enemy targetEnemy;
    public bool testing;
    public bool win;

    public Transform t;
    [SerializeField] Transform door;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Transform srt;
    [SerializeField] Sprite[] s;
    Vector3 last;
    public WeaponSO w;
    public int mhp;
    public int hp;
    public int hpTimer;
    public int regenRate;
    public int regenAmount;
    [SerializeField] Transform hpbar;
    public Inventory inven;
    int damageDealt;
    
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
        if (!GameManager.isPaused) {
            GameManager.timer += Time.deltaTime;
            GameManager.moveTimer += Time.deltaTime;
        } else {
            if (GameManager.showAnims){
                GameManager.timer += Time.deltaTime;
                if (win) {
                    if (GameManager.timer > 0.5f) {
                    GameManager.timer = 0.5f;
                    hpbar.localPosition = Vector3.zero;
                    }
                }
                
            } else {
                GameManager.delay -= Time.deltaTime;
                if (GameManager.delay <= 0f) {
                    GameManager.showAnims = true;
                }
            }
            
        }

        if (GameManager.timer >= 0.5f) {
            FindObjectOfType<Treasury>().addCoin(damageDealt / 5);
            damageDealt -= (damageDealt / 5) * 5;
        }

        if (GameManager.timer >= 1f || (GameManager.timer >= 0.5f && !Util.enemiesMoving())) {
            GameManager.timer = 1f;
            hpbar.localScale = new Vector3((hp/(float) mhp) * (10/16f), (2/16f), 1);
        }

        if (GameManager.moveTimer >= GameManager.moveInterval && !GameManager.isPaused) {
            GameManager.moveTimer = 0f;
            determineTarget();
            move(playerPathfind());
        }

        if (GameManager.isPaused) {
            GameManager.moveTimer = GameManager.moveInterval;
        }
        
        if (GameManager.timer < 0.5f) {
            Util.moveSprite(srt, last, t.position, GameManager.timer);
        } else {
            Util.moveSprite(srt, last, t.position, 0.5f);
        }
    }

    public void determineTarget() {
        if (t.position == ((Util.getD().position - new Vector3(0, 1, 0)))) {
            target = "enterDoor";
            return;
        }

        if (Pathfind.pathfind(t.position, Util.getD().position, 1, 4, 0, null, false, false) != t.position 
            && !GameManager.isEnemyInSight(3)) {
            target = "moveToDoor";
            return;
        }

        if (GameManager.isEnemyInSight(4) || targetEnemy != null) {
            target = "attackEnemy";
            targetEnemy = GameManager.closestEnemyInSight();
            return;
        }

        if (GameManager.isItemInSight(25)) {
            target = "retrieveItem";
            return;
        }

        if (GameManager.isEnemyInSight(7)) {
            target = "attackEnemy";
            targetEnemy = GameManager.closestEnemyInSight();
            return;
        }

        target = "wander";
    }

    public void setPos(Vector2 v) {
        last = t.position;
        t.position = new Vector3(v.x, v.y, 0f);
    }

    public void move(Vector2 v) {
        if (Util.isDoor(v)) {
            GameManager.isPaused = true;
            GameManager.delay = 1f;
            win = true;
            FindObjectOfType<DragCamera>().cameraControl = false;
            FindObjectOfType<DragCamera>().cameraTarget = Util.getD().position + new Vector3(0f, 0f, -10f);
            FindObjectOfType<DragCamera>().timeSinceTarget = 0f;
        }
        if (Util.itemOccupying(v) != null) {
            Util.itemOccupying(v).w.atk += (int) Util.itemOccupying(v).mods.x;
            Util.itemOccupying(v).w.ran += (int) Util.itemOccupying(v).mods.y;
            Destroy(Util.itemOccupying(v).gameObject);
        }
        Util.face(t.position, v, sr, s);
        setPos(v);
        Util.moveSprite(srt, last, t.position, 0f);

        hpTimer++;
        if (hpTimer >= regenRate) {
            hp += regenAmount;
            if (hp > mhp) {
                hp = mhp;
            }
            hpTimer = 0;
        } 

        GameManager.onPlayerAction();
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

    public void damage(int damage, bool healSickness) {
        if (!healSickness) {
            damageDealt += damage;
        }

        hp -= damage;
        if (hp <= 0) {
            Debug.Log("ded");
            hp = 0;
        }
    }

    public Vector2 playerPathfind() {
        Vector2 result = new Vector2();

        switch (target) {
            
            case "enterDoor":
                result = Util.getD().position;
                break;
            
            case "attackEnemy":
            inven.updateInven();
                if (GameManager.isEnemyInSight(7)) {
                    result = Pathfind.pathfind(t.position, GameManager.closestEnemyInSight().t.position, 1, 15, w.ran, null, true, false);
                } else {
                    result = Pathfind.pathfind(t.position, GameManager.closestEnemy().t.position, 1, 25, 0, null, false, false);
                    if (result == (Vector2) t.position) {
                        target = "moveToDoor";
                        result = playerPathfind();
                    }
                }
                break;

            case "retrieveItem":
                if (GameManager.isItemInSight(25)) {
                    result = Pathfind.pathfind(t.position, GameManager.closestItemInSight().t.position, 1, 25, 0, null, false, false);
                } else {
                    result = Pathfind.pathfind(t.position, GameManager.closestItem().t.position, 1, 25, 0, null, false, false);
                }
                if (result == (Vector2) t.position) {
                    target = "moveToDoor";
                    result = playerPathfind();
                }
                break;

            case "moveToDoor":
            default:
                result = Pathfind.pathfind(t.position, Util.getD().position, 1, 0, null, false, true);
                break;
        }

        return result;
    }
}
