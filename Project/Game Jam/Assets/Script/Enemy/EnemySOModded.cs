using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySOModded {
    public Sprite s;
    public int hp;
    public int def;
    public int spd;
    public int vran;
    public int atk;
    public int ran;
    public Sprite atks;

    public EnemySOModded(EnemySO e) {
        s = e.s;
        hp = e.hp;
        def = e.def;
        spd = e.spd;
        vran = e.vran;
        atk = e.atk;
        ran = e.ran;
        atks = e.atks;
    }

    public void applyMods(EnemyMods e) {
        this.hp += e.hp;
        this.def += e.def;
        this.spd += e.spd;
        this.vran += e.vran;
        this.atk += e.atk;
        this.ran += e.ran;
    }
}
