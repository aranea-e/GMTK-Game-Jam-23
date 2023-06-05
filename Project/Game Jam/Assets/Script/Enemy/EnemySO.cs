using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Enemy", order = 2)]
public class EnemySO : ScriptableObject {
    public Sprite s;
    public int hp;
    public int def;
    public int spd;
    public int vran;
    public int atk;
    public int ran;
    public Sprite atks;
}
