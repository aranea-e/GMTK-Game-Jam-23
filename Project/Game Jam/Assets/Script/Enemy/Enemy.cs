using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public EnemySOModded so;
    public EnemySO soBase;
    public Transform t;
    public Transform srt;
    public SpriteRenderer sr;
    public Sprite[] s;
    float timer;
    public Vector3 last;
    public int hp;
    public bool moving;
    [SerializeField] Transform hpbar;

    public int baseSizeOnGen;
    public int roomNumberOnGen;
    public int roomCountOnGen;

    void Start() {
        so = new EnemySOModded(soBase);
        so.applyMods(generateMods(baseSizeOnGen, roomNumberOnGen, roomCountOnGen));
        last = t.position;
        hp = so.hp;
    }

    public void Update() {
        if (GameManager.timer >= 0.5f) {
            Util.moveSprite(srt, last, t.position, GameManager.timer - 0.5f);
        } else {
            Util.moveSprite(srt, last, t.position, 0f);
        }
        hpbar.localScale = new Vector3((hp/(float) so.hp) * (10/16f), (2/16f), 1);
    }

    public void setFGHVals(int baseSize, int roomNumber, int roomCount) {
        baseSizeOnGen = baseSize;
        roomNumberOnGen = roomNumber;
        roomCountOnGen = roomCount;
    }
    
    public void pathfind() {
        moveTo(Pathfind.pathfind(t.position, Util.getP().t.position, so.spd, so.vran, so.ran, this, true));
    }

    void moveTo(Vector3 p) {
        last = t.position;
        Util.face(t.position, p, sr, s);
        t.position = p;
    }

    public void attack(Vector3 target) {
        GameManager.attackAnim((so.ran > 1), false, so.atks, t.position, target);
        Util.getP().damage(so.atk);
    }

    public void damage(int damage) {
        if (damage > so.def) {
            hp -= damage - so.def;
        }

        if (hp <= 0) {
            Destroy(gameObject);
        }
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
