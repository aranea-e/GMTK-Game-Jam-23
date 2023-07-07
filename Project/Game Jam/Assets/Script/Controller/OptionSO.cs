using UnityEngine;

[CreateAssetMenu(fileName = "Option", menuName = "Option", order = 4)]
public class OptionSO : ScriptableObject {
    public string n;
    public int price;
    public Sprite s;
    public Vector2[] targets;
}