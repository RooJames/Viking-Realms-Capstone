using System;
using UnityEngine;

public class PlayerTalents : MonoBehaviour
{
    public static event Action<int> OnPointsChanged;

    [SerializeField] private int talentPoints = 0;

    public int TalentPoints => talentPoints;

    public void AddPoints(int amount)
    {
        talentPoints += amount;
        OnPointsChanged?.Invoke(talentPoints);
    }

    public void SpendPoint()
    {
        if (talentPoints <= 0)
            return;

        talentPoints--;
        OnPointsChanged?.Invoke(talentPoints);
    }
}