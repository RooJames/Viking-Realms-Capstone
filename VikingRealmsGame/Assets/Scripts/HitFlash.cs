using UnityEngine;

public class HitFlash : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color flashColor = Color.orangeRed;
    [SerializeField] private float flashDuration = 0.1f;

    private Color originalColor;
    private Health health;
    private float flashTimer;
    private bool isFlashing;

    private void Awake()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        originalColor = spriteRenderer.color;
        health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        if (health != null)
            health.OnDamaged.AddListener(HandleHit);
    }

    private void OnDisable()
    {
        if (health != null)
            health.OnDamaged.RemoveListener(HandleHit);
    }

    private void HandleHit(float damage)
    {
        isFlashing = true;
        flashTimer = flashDuration;
        spriteRenderer.color = flashColor;
    }

    private void Update()
    {
        if (!isFlashing) return;

        flashTimer -= Time.deltaTime;
        if (flashTimer <= 0f)
        {
            spriteRenderer.color = originalColor;
            isFlashing = false;
        }
    }
}
