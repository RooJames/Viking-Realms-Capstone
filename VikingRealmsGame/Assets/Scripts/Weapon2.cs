using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damage = 1;

    public enum WeaponType { Melee, Bullet }
    public WeaponType weaponType = WeaponType.Melee;

    private HashSet<Collider2D> hitThisSwing = new HashSet<Collider2D>();

    private void OnEnable()
    {
        hitThisSwing.Clear();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (weaponType == WeaponType.Melee)
        {
            if (hitThisSwing.Contains(collision)) return;
            hitThisSwing.Add(collision);
        }

        Health hp = collision.GetComponent<Health>();
        if (hp != null)
        {
            hp.TakeDamage(Mathf.RoundToInt(damage));

            if (weaponType == WeaponType.Bullet)
                Destroy(gameObject);
        }
    }
}
