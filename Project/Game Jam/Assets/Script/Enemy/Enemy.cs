using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public EnemySOModded so;
    public EnemySO soBase;
    public SpriteRenderer optionSprite;
    public Transform t;
    public Transform srt;
    public SpriteRenderer sr;
    public Sprite[] s;
    float timer;
    public Vector3 last;
    public int hp;
    public bool markForDestuction;
    public bool moving;
    [SerializeField] Transform hpbar;
    public bool healSickness;

    public int baseSizeOnGen;
    public int roomNumberOnGen;
    public int roomCountOnGen;

    public OptionSO currentOption;
    public static OptionSO[] options;
    public List<Vector2> moveTargets;
    public int currentTarget;

    void Start() {
        so = new EnemySOModded(soBase);
        so.applyMods(generateMods(baseSizeOnGen, roomNumberOnGen, roomCountOnGen));
        last = t.position;
        hp = so.hp;
    }

    public void Update() {
        if (GameManager.timer >= (GameManager.moveInterval / 2f)) {
            if (markForDestuction == true) {
                Destroy(gameObject);
            }
            Util.moveSprite(srt, last, t.position, (GameManager.timer - (GameManager.moveInterval / 2f)) / GameManager.moveInterval);
            hpbar.localScale = new Vector3((hp/(float) so.hp) * (10/16f), (2/16f), 1);
        } else {
            Util.moveSprite(srt, last, t.position, 0f);
        }
        optionSprite.sprite = currentOption.s;
    }

    public void setFGHVals(int baseSize, int roomNumber, int roomCount) {
        baseSizeOnGen = baseSize;
        roomNumberOnGen = roomNumber;
        roomCountOnGen = roomCount;
    }
    
    public void pathfind() {
        switch (currentOption.n) {
            case "attack":
                moveTo(Pathfind.pathfind(t.position, Util.getP().t.position, so.spd, so.ran, this, true, false));
                break;
            
            case "move":
                if (Util.lineOfSight(t.position, Util.getP().t.position) && Pathfind.isInRange(t.position, Util.getP().t.position, so.vran)) {
                    currentOption = options[1];
                    pathfind();
                    return;
                }
                if (moveTargets.Count > 0 && Util.tileAvailable(moveTargets[0])) {
                    moveTo(Pathfind.pathfind(t.position, moveTargets[0], so.spd, so.ran, this, false, false));
                    if (moveTargets[0] == (Vector2) t.position) {
                        currentOption = options[0];
                    }
                }
                break;
            
            case "patrol":
                if (Util.lineOfSight(t.position, Util.getP().t.position) && Pathfind.isInRange(t.position, Util.getP().t.position, so.vran)) {
                    currentOption = options[1];
                    pathfind();
                    return;
                }
                if (moveTargets.Count > currentTarget) {
                    if (Util.tileAvailable(moveTargets[currentTarget])){
                        moveTo(Pathfind.pathfind(t.position, moveTargets[currentTarget], so.spd, so.ran, this, false, false));
                    }
                    if (moveTargets[currentTarget] == (Vector2) t.position) {
                        currentTarget++;
                    }
                } else {
                    currentTarget = 0;
                    pathfind();
                }
                break;
            
            case "guard":
            default:
                moveTo(Pathfind.pathfind(t.position, Util.getP().t.position, so.spd, so.vran, so.ran, this, true, false));
                break;
        }
    }

    void moveTo(Vector3 p) {
        last = t.position;
        Util.face(t.position, p, sr, s);
        t.position = p;
    }

    public void attack(Vector3 target) {
        GameManager.attackAnim((so.ran > 1), false, so.atks, t.position, target);
        Util.getP().damage(so.atk, healSickness);
        healSickness = false;
    }

    public void damage(int damage) {
        if (damage > so.def) {
            hp -= damage - so.def;
        }

        if (hp <= 0) {
            markForDestuction = true;
        }
    }

    public void heal() {
        hp = so.hp;
        healSickness = true;
    }

    public EnemyMods generateMods(int factor1, int factor2, int factor3) {
        EnemyMods result = new EnemyMods();
        float percent = (factor2 / (float) factor3);

        #region hp
        int maxIncrease = 15 - so.hp;
        float maxIncreaseByLevel = factor1 / 2f;
        float percentDeviation = .5f;
        
        int hpMod = (int) (maxIncreaseByLevel * adjustedPercent(percent, percentDeviation));
        if (hpMod < 0) {
            hpMod = 0;
        }
        if (hpMod > maxIncrease) {
            hpMod = maxIncrease;
        }
        #endregion
        #region def
        maxIncrease = 5 - so.def;
        maxIncreaseByLevel = factor1 / 10f;
        percentDeviation = .25f;
        
        int defMod = (int) (maxIncreaseByLevel * adjustedPercent(percent, percentDeviation));
        if (defMod < 0) {
            defMod = 0;
        }
        if (defMod > maxIncrease) {
            defMod = maxIncrease;
        }
        #endregion
        #region spd
        int spdMod = 0;
        #endregion
        #region vran
        maxIncrease = 9 - so.vran;
        maxIncreaseByLevel = factor1 / 3f;
        percentDeviation = .4f;
        
        int vranMod = (int) (maxIncreaseByLevel * adjustedPercent(percent, percentDeviation));
        if (vranMod < 0) {
            vranMod = 0;
        }
        if (vranMod > maxIncrease) {
            vranMod = maxIncrease;
        }
        #endregion
        #region atk
        maxIncrease = 9 - so.atk;
        maxIncreaseByLevel = factor1 / 4f;
        percentDeviation = .4f;
        
        int atkMod = (int) (maxIncreaseByLevel * adjustedPercent(percent, percentDeviation));
        if (atkMod < 0) {
            atkMod = 0;
        }
        if (atkMod > maxIncrease) {
            atkMod = maxIncrease;
        }
        #endregion
        #region ran
        int ranMod = 0;
        if (so.ran != 1) {
            maxIncrease = 7 - so.ran;
            maxIncreaseByLevel = factor1 / 10f;
            percentDeviation = .4f;
            
            ranMod = (int) (maxIncreaseByLevel * adjustedPercent(percent, percentDeviation));
            if (ranMod < 0) {
                ranMod = 0;
            }
            if (ranMod > maxIncrease) {
                ranMod = maxIncrease;
            }
        }
        #endregion
        result.hp = hpMod;
        result.def = defMod;
        result.spd = spdMod;
        result.vran = vranMod;
        result.atk = atkMod;
        result.ran = ranMod;
        return result;
    }

    float adjustedPercent(float percent, float percentDeviation) {
        percentDeviation = Random.Range(0, percentDeviation);
        return percent + percentDeviation;
    }
}
