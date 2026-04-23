using UnityEngine;
using System;

public class PlayerStats : MonoBehaviour
{
    [HideInInspector] public bool isExhausted = false;

    // EVENTS
    public event Action<float, float> OnHealthChanged;
    public event Action<float, float> OnEnergyChanged;
    public event Action<float, float> OnXPChanged;
    public event Action<int> OnLevelChanged;
    public event Action<int> OnTalentPointsChanged;
    public event Action OnDamageTaken;
    public event Action OnHealed;

    [Header("Health")]
    public float maxHealth = 100f;
    public float currentHealth = 100f;

    [Header("Energy")]
    public float maxEnergy = 60f;
    public float currentEnergy = 60f;

    [Header("Experience")]
    public float currentXP = 0f;
    public float xpToNextLevel = 100f;

    [Header("Level")]
    public int level = 1;

    [Header("Talents")]
    public int talentPoints = 0;

    // HEALTH
    public void TakeDamage(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        OnDamageTaken?.Invoke();
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        OnHealed?.Invoke();
    }

    // ENERGY
    public void UseEnergy(float amount)
    {
        currentEnergy = Mathf.Clamp(currentEnergy - amount, 0, maxEnergy);
        OnEnergyChanged?.Invoke(currentEnergy, maxEnergy);
    }

    public void RegenEnergy(float amount)
    {
        currentEnergy = Mathf.Clamp(currentEnergy + amount, 0, maxEnergy);
        OnEnergyChanged?.Invoke(currentEnergy, maxEnergy);
    }

    // EXPERIENCE + LEVELING + TALENTS
    public void GainXP(float amount)
    {
        currentXP += amount;

        // Handle multiple level-ups in one gain
        while (currentXP >= xpToNextLevel)
        {
            currentXP -= xpToNextLevel;
            LevelUp();
        }

        OnXPChanged?.Invoke(currentXP, xpToNextLevel);
    }

    private void LevelUp()
    {
        level++;
        talentPoints++;

        // Optional: increase XP requirement each level
        xpToNextLevel *= 1.15f;

        OnLevelChanged?.Invoke(level);
        OnTalentPointsChanged?.Invoke(talentPoints);
    }

    // TALENT SPENDING
    public bool SpendTalentPoint()
    {
        if (talentPoints <= 0)
            return false;

        talentPoints--;
        OnTalentPointsChanged?.Invoke(talentPoints);
        return true;
    }

    private void Update()
    {
        // Debug keys
        if (Input.GetKeyDown(KeyCode.K) && !Input.GetKey(KeyCode.LeftShift))
            TakeDamage(10);

        if (Input.GetKeyDown(KeyCode.K) && Input.GetKey(KeyCode.LeftShift))
            Heal(10);

        // XP debug key
        if (Input.GetKeyDown(KeyCode.L))
            GainXP(25);

        // ENERGY DRAIN WHILE MOVING
        bool isMoving = Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0;

        if (isMoving)
            DrainEnergyOverTime();
        else
            RegenerateEnergyOverTime();
    }

    private void DrainEnergyOverTime()
    {
        float drainRate = 10f;
        currentEnergy -= drainRate * Time.deltaTime;
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);

        // Exhausted when empty
        isExhausted = currentEnergy <= 0f;

        OnEnergyChanged?.Invoke(currentEnergy, maxEnergy);
    }

    private void RegenerateEnergyOverTime()
    {
        float regenRate = 15f;
        currentEnergy += regenRate * Time.deltaTime;
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);

        // No longer exhausted once energy rises above 0
        if (currentEnergy > 0f)
            isExhausted = false;

        OnEnergyChanged?.Invoke(currentEnergy, maxEnergy);
    }
    
    public void RefreshStatsUI()
    {
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        OnEnergyChanged?.Invoke(currentEnergy, maxEnergy);
        OnXPChanged?.Invoke(currentXP, xpToNextLevel);
        OnLevelChanged?.Invoke(level);
        OnTalentPointsChanged?.Invoke(talentPoints);
    }

}

