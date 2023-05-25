using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class AI {
    public virtual Vector3 pathfind(Vector3 s, Vector3 t, int spd, int search) {
        return Vector3.zero;
    }
}
public class Basic : AI {
    public override Vector3 pathfind(Vector3 s, Vector3 t, int spd, int vran) {
        return Pathfind.pathfind(s, t, spd, vran);
    }
}
