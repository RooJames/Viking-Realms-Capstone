using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Health : MonoBehaviour, IDamageable
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private bool destroyOnDeath = true;
    [SerializeField] private float invincibilityDuration = 0.5f; // NEW

    [Header("Events")]
    public UnityEvent<float, float> OnHealthChanged; 
    public UnityEvent<float> OnDamaged;              
    public UnityEvent OnDeath;

    [Header("Damage SFX")]
    public AudioSource damageAudioSource;
    public AudioClip damageSfx;

    public float MaxHealth => maxHealth;
    public float CurrentHealth { get; private set; }
    public bool IsDead { get; private set; }

    private bool isInvincible;              // NEW
    private float invincibleEndTime;        // NEW
    private SpriteRenderer sr;              // NEW

    private void Awake()
    {
        CurrentHealth = maxHealth;
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);

        sr = GetComponent<SpriteRenderer>(); // NEW
    }

    private void Update()
    {
        // Turn off invincibility when timer ends
        if (isInvincible && Time.time >= invincibleEndTime)
            isInvincible = false;
    }

    public void TakeDamage(float amount)
    {
        if (IsDead || amount <= 0f) return;

        // NEW: block damage during i-frames
        if (isInvincible) return;

        CurrentHealth = Mathf.Clamp(CurrentHealth - amount, 0f, maxHealth);

        if (GSController.sfxOn && damageAudioSource && damageSfx)
        {
            damageAudioSource.PlayOneShot(damageSfx);
        }

        OnDamaged?.Invoke(amount);
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);

        // NEW: start i-frames
        StartInvincibility();

        if (CurrentHealth <= 0f && !IsDead)
        {
            Die();
        }
    }

    private void StartInvincibility() // NEW
    {
        isInvincible = true;
        invincibleEndTime = Time.time + invincibilityDuration;

        if (sr != null)
            StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine() // NEW
    {
        while (isInvincible)
        {
            sr.enabled = false;
            yield return new WaitForSeconds(0.1f);
            sr.enabled = true;
            yield return new WaitForSeconds(0.1f);
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