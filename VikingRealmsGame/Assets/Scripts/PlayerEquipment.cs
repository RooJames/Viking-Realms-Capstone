using UnityEngine;

public class PlayerEquipment : MonoBehaviour {
    public ToolBelt equippedBelt;

    public int GetFreeSlotCount() {
        return equippedBelt != null ? equippedBelt.extraSlots : 0;
    }
}