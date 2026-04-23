using System;
using UnityEngine;

[Serializable]
public class QuestReward
{
    [Tooltip("Item prefab to add to the player's inventory")]
    public GameObject itemPrefab;

    [Tooltip("How many copies to give")]
    public int quantity = 1;
}
