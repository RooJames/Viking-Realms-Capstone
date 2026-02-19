using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] private float damageAmount = 10f;
    [SerializeField] private bool destroyOnHit = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damageAmount);

            if (destroyOnHit)
                Destroy(gameObject);
        }
    }
}