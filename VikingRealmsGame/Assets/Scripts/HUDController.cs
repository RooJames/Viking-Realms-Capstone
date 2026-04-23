using UnityEngine;
using TMPro;

public class HUDController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerStats stats;

    [Header("Health UI")]
    [SerializeField] private BarFillAnimator healthFill;
    [SerializeField] private HealthColorGradient healthColor;
    [SerializeField] private BarDamageFlash damageFlash;
    [SerializeField] private BarHealGlow healGlow;
    [SerializeField] private BarLowHealthPulse lowHealthPulse;
    [SerializeField] private TMP_Text healthText;

    [Header("Energy UI")]
    [SerializeField] private BarFillAnimator energyFill;
    [SerializeField] private EnergyColorGradient energyColor;
    [SerializeField] private BarEnergyShimmer energyShimmer;
    [SerializeField] private TMP_Text energyText;

    [Header("XP UI")]
    [SerializeField] private BarFillAnimator xpFill;
    [SerializeField] private TMP_Text xpText;
    [SerializeField] private XPColorGradient xpColor;
    [SerializeField] private XPFlashEffect xpFlash;


    [Header("Level + Talents")]
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text talentPointsText;

    private void Start()
    {
        // Safety check — prevents silent failures
        if (stats == null)
        {
            Debug.LogError("HUDController: PlayerStats reference is missing!");
            return;
        }

        // Subscribe to stat events
        stats.OnHealthChanged += UpdateHealth;
        stats.OnEnergyChanged += UpdateEnergy;
        stats.OnXPChanged += UpdateXP;
        stats.OnLevelChanged += UpdateLevel;
        stats.OnTalentPointsChanged += UpdateTalentPoints;

        stats.OnDamageTaken += () => damageFlash.Flash();
        stats.OnHealed += () => healGlow.Glow();

        // Initialize UI immediately
        UpdateHealth(stats.currentHealth, stats.maxHealth);
        UpdateEnergy(stats.currentEnergy, stats.maxEnergy);
        UpdateXP(stats.currentXP, stats.xpToNextLevel);
        UpdateLevel(stats.level);
        UpdateTalentPoints(stats.talentPoints);
    }

    private void UpdateHealth(float current, float max)
    {
        float normalized = current / max;

        healthFill.SetFill(normalized);
        healthColor.UpdateColor(normalized);

        healthText.text = $"{Mathf.RoundToInt(current)}/{Mathf.RoundToInt(max)}";

        lowHealthPulse.SetPulsing(normalized <= 0.2f);
    }

    private void UpdateEnergy(float current, float max)
    {
        Debug.Log("UpdateEnergy called. Text ref = " + energyText);

        float normalized = current / max;

        energyFill.SetFill(normalized);
        energyColor.UpdateColor(normalized);

        energyText.text = $"{Mathf.RoundToInt(current)}/{Mathf.RoundToInt(max)}";

        energyShimmer.SetShimmer(normalized < 1f);
    }

    private void UpdateXP(float current, float required)
    {
        float normalized = current / required;

        xpFill.SetFill(normalized);
        xpText.text = $"{Mathf.RoundToInt(current)}/{Mathf.RoundToInt(required)}";

        xpColor.UpdateColor(normalized);

        // Flash when XP bar fills completely
        if (normalized >= 1f)
            xpFlash.Flash();
    }


    private void UpdateLevel(int level)
    {
        levelText.text = level.ToString();
    }

    private void UpdateTalentPoints(int points)
    {
        talentPointsText.text = points.ToString();
    }
    
    public void ForceRefresh(PlayerStats stats)
    {
        UpdateHealth(stats.currentHealth, stats.maxHealth);
        UpdateEnergy(stats.currentEnergy, stats.maxEnergy);
        UpdateXP(stats.currentXP, stats.xpToNextLevel);
        UpdateLevel(stats.level);
        UpdateTalentPoints(stats.talentPoints);
    }

}
