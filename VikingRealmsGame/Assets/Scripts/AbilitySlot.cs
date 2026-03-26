using UnityEngine;
using UnityEngine.UI;

public enum AbilitySlotType {
    Preset,
    Free
}

public class AbilitySlot : MonoBehaviour {
    public AbilitySlotType slotType;
    public Image icon;
    public Ability assignedAbility;

    public bool IsFree => slotType == AbilitySlotType.Free;

    public void SetAbility(Ability ability) {
        assignedAbility = ability;
        icon.sprite = ability.icon;
        icon.enabled = ability != null;
    }

    public void Clear() {
        if (!IsFree) return;
        assignedAbility = null;
        icon.enabled = false;
    }
}