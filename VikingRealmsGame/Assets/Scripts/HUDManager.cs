using UnityEngine;

public class HUDManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private UIHealthBar healthBar;
    [SerializeField] private UIDashCooldown dashCooldown;
    [SerializeField] private UIXPBar xpBar;
    [SerializeField] private UITalentPoints talentPoints;

    private void Awake()
    {
        if (!healthBar) healthBar = GetComponentInChildren<UIHealthBar>(true);
        if (!dashCooldown) dashCooldown = GetComponentInChildren<UIDashCooldown>(true);
        if (!xpBar) xpBar = GetComponentInChildren<UIXPBar>(true);
        if (!talentPoints) talentPoints = GetComponentInChildren<UITalentPoints>(true);
    }
}