using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, IDamageable
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private bool destroyOnDeath = true;

    [Header("Events")]
    public UnityEvent<float, float> OnHealthChanged; // current, max
    public UnityEvent<float> OnDamaged;              // damage amount
    public UnityEvent OnDeath;

    public float MaxHealth => maxHealth;
    public float CurrentHealth { get; private set; }
    public bool IsDead { get; private set; }

    private void Awake()
    {
        CurrentHealth = maxHealth;
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
    }

    public void TakeDamage(float amount)
    {
        if (IsDead || amount <= 0f) return;

        CurrentHealth = Mathf.Clamp(CurrentHealth - amount, 0f, maxHealth);
        OnDamaged?.Invoke(amount);
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);

        if (CurrentHealth <= 0f && !IsDead)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (IsDead || amount <= 0f) return;

        CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0f, maxHealth);
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
    }

    private void Die()
    {
        IsDead = true;
        OnDeath?.Invoke();

        if (destroyOnDeath)
        {
            Destroy(gameObject, 0.1f);
        }
    }
}