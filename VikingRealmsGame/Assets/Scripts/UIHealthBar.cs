using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    [SerializeField] private Health targetHealth;
    [SerializeField] private Image fillImage;

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

    private void UpdateBar(float current, float max)
    {
        fillImage.fillAmount = current / max;
    }
}