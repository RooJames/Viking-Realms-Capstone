using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BarHealGlow : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private float glowTime = 0.2f;

    public void Glow()
    {
        StartCoroutine(GlowRoutine());
    }

    private IEnumerator GlowRoutine()
    {
        Color original = fillImage.color;
        Color glow = original * 1.5f;

        fillImage.color = glow;
        yield return new WaitForSeconds(glowTime);
        fillImage.color = original;
    }
}