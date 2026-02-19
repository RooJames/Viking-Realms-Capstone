using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Health targetHealth;
    [SerializeField] private Slider slider;

    private void Awake()
    {
        if (slider == null)
            slider = GetComponent<Slider>();
    }

    private void OnEnable()
    {
        if (targetHealth != null)
        {
            targetHealth.OnHealthChanged.AddListener(UpdateBar);
            UpdateBar(targetHealth.CurrentHealth, targetHealth.MaxHealth);
        }
    }

    private void OnDisable()
    {
        if (targetHealth != null)
            targetHealth.OnHealthChanged.RemoveListener(UpdateBar);
    }

    public void UpdateBar(float current, float max)
    {
        slider.maxValue = max;
        slider.value = current;
    }
}