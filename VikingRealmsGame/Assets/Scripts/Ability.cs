using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Ability")]
public class Ability : ScriptableObject {
    public string abilityName;
    public Sprite icon;
    // Later: cooldown, damage, type, etc.
}
