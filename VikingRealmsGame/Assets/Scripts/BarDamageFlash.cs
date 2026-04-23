using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BarDamageFlash : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private float flashDuration = 0.1f;

    public void Flash()
    {
        StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        Color original = fillImage.color;
        fillImage.color = Color.white;
        yield return new WaitForSeconds(flashDuration);
        fillImage.color = original;
    }
}