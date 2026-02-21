using UnityEngine;

public class DamageTextSpawner : MonoBehaviour
{
    [SerializeField] private FloatingDamageText damageTextPrefab;
    [SerializeField] private Canvas worldCanvas;
    [SerializeField] private Vector3 offset = new Vector3(0, 1f, 0);

    private Camera mainCam;
    private Health health;

    private void Awake()
    {
        mainCam = Camera.main;
        health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        if (health != null)
            health.OnDamaged.AddListener(SpawnDamageText);
    }

    private void OnDisable()
    {
        if (health != null)
            health.OnDamaged.RemoveListener(SpawnDamageText);
    }

    public void SpawnDamageText(float damage)
    {
        if (damageTextPrefab == null || worldCanvas == null) return;

        Vector3 worldPos = transform.position + offset;
        Vector3 screenPos = mainCam.WorldToScreenPoint(worldPos);

        FloatingDamageText instance =
            Instantiate(damageTextPrefab, worldCanvas.transform);
        instance.transform.position = screenPos;
        instance.Initialize(damage);
    }
}