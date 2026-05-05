using UnityEngine;
using UnityEngine.Events;

public class OrcHealth : MonoBehaviour, IDamageable
{
    [Header("Health")]
    public float maxHealth = 50f;
    public float currentHealth;

    [Header("XP Reward")]
    public float xpReward = 30f;

    [Header("Events")]
    public UnityEvent OnDeath;

    private bool isDead = false;

    public bool IsDead => isDead;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void Heal(float amount) { /* orcs don't heal */ }

    public void TakeDamage(float amount)
    {
        if (isDead || amount <= 0f) return;

        currentHealth = Mathf.Clamp(currentHealth - amount, 0f, maxHealth);

        if (currentHealth <= 0f)
            Die();
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        // Award XP to the player via PlayerStats
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            PlayerStats stats = playerObj.GetComponent<PlayerStats>();
            if (stats != null)
                stats.GainXP(xpReward);
        }

        OnDeath?.Invoke();
        Destroy(gameObject);
    }
}