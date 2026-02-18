using UnityEngine;

public class Health2 : MonoBehaviour
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
            Dead();
    }

    void Dead()
    {
        Destroy(gameObject);
    }
}
