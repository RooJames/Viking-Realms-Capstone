using System;
using UnityEngine;

public class PlayerXP : MonoBehaviour
{
    public static event Action<float, float> OnXPChanged;
    public static event Action<int> OnLevelUp;

    [SerializeField] private int level = 1;
    [SerializeField] private float currentXP = 0f;
    [SerializeField] private float xpToNextLevel = 100f;

    public int Level => level;
    public float CurrentXP => currentXP;
    public float XPToNextLevel => xpToNextLevel;

    public void AddXP(float amount)
    {
        currentXP += amount;
        OnXPChanged?.Invoke(currentXP, xpToNextLevel);

        if (currentXP >= xpToNextLevel)
            LevelUp();
    }

    private void LevelUp()
    {
        level++;
        currentXP = 0f;
        xpToNextLevel *= 1.25f; // scale requirement

        OnLevelUp?.Invoke(level);
        OnXPChanged?.Invoke(currentXP, xpToNextLevel);
    }
}