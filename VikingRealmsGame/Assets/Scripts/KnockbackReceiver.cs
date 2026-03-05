using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class KnockbackReceiver : MonoBehaviour
{
    [Header("Knockback Settings")]
    [SerializeField] private float knockbackResistance = 0f;   // 0 = full force, 1 = immune
    [SerializeField] private float hitStunDuration = 0.15f;

    private Rigidbody2D rb;
    private bool inHitStun;
    private float hitStunEndTime;

    public bool InHitStun => inHitStun;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void ApplyKnockback(Vector2 direction, float force)
    {
        float finalForce = force * (1f - knockbackResistance);

        // Reset velocity so knockback is consistent
        rb.linearVelocity = Vector2.zero;

        // Apply impulse
        rb.AddForce(direction.normalized * finalForce, ForceMode2D.Impulse);

        // Start hit-stun
        inHitStun = true;
        hitStunEndTime = Time.time + hitStunDuration;
    }

    private void Update()
    {
        if (inHitStun && Time.time >= hitStunEndTime)
        {
            inHitStun = false;
        }
    }
}