using UnityEngine;
using System.Collections.Generic;

public class AbilityRing : MonoBehaviour {
    public RadialLayoutGroup layoutGroup;
    public GameObject abilitySlotPrefab;

    public Ability swordAbility;
    public Ability staffAbility;
    public Ability bowAbility;

    public PlayerEquipment playerEquipment;

    public List<AbilitySlot> slots = new List<AbilitySlot>();

    void Start() {
        InitializeRing();
    }

    public void InitializeRing() {
        int freeSlotCount = playerEquipment.GetFreeSlotCount();

        foreach (Transform child in transform)
            Destroy(child.gameObject);

        slots.Clear();

        CreateSlot(AbilitySlotType.Preset, swordAbility);
        CreateSlot(AbilitySlotType.Preset, staffAbility);
        CreateSlot(AbilitySlotType.Preset, bowAbility);

        for (int i = 0; i < freeSlotCount; i++)
            CreateSlot(AbilitySlotType.Free, null);

        layoutGroup.UpdateLayout();
    }

    private void CreateSlot(AbilitySlotType type, Ability ability) {
        GameObject obj = Instantiate(abilitySlotPrefab, transform);
        AbilitySlot slot = obj.GetComponent<AbilitySlot>();
        slot.slotType = type;

        if (ability != null)
            slot.SetAbility(ability);

        slots.Add(slot);
    }
}