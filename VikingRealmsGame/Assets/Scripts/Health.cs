using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHP = 5;
    public int currentHP;

    void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;

        if (currentHP <= 0)
            Die();
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
