using UnityEngine;

// Attach to any enemy or animal prefab.
// The entityTag must match QuestObjective.targetID exactly (e.g. "Orc", "Deer").
[RequireComponent(typeof(Health))]
public class QuestEntityTag : MonoBehaviour
{
    [Tooltip("Must match the targetID on the KillEnemy objective exactly")]
    public string entityTag;

    private Health _health;

    void Awake()
    {
        _health = GetComponent<Health>();
    }

    void OnEnable()
    {
        if (_health != null)
            _health.OnDeath.AddListener(OnDied);
    }

    void OnDisable()
    {
        if (_health != null)
            _health.OnDeath.RemoveListener(OnDied);
    }

    private void OnDied()
    {
        QuestManager.Instance?.TrackKill(entityTag);
    }
}
