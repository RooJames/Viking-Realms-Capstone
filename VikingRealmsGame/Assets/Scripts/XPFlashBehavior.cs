using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class XPFlashEffect : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private float flashTime = 0.25f;
    [SerializeField] private Color flashColor = Color.white;

    private Color originalColor;
    private Coroutine flashRoutine;

    void Start()
    {
        if (fillImage != null)
            originalColor = fillImage.color;
    }

    public void Flash()
    {
        if (flashRoutine != null)
            StopCoroutine(flashRoutine);

        flashRoutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        fillImage.color = flashColor;
        yield return new WaitForSeconds(flashTime);
        fillImage.color = originalColor;
    }
}