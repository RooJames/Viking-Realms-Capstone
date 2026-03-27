using UnityEngine;

[CreateAssetMenu(menuName = "Items/Tool Belt")]
public class ToolBelt : ScriptableObject {
    public string beltName;
    public int extraSlots; // number of free ability slots
    public Sprite icon;
}