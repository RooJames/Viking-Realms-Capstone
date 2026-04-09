using UnityEngine;
using UnityEngine.UI;

public class UIDashCooldown : MonoBehaviour
{
    [SerializeField] private Image radialFill;

    private void OnEnable()
    {
        PlayerDash.OnCooldownUpdate += UpdateCooldown;
    }

    private void OnDisable()
    {
        PlayerDash.OnCooldownUpdate -= UpdateCooldown;
    }

    private void UpdateCooldown(float timeLeft, float cooldown)
    {
        radialFill.fillAmount = 1f - (timeLeft / cooldown);
    }
}