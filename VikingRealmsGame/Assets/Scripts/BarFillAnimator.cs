using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BarFillAnimator : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private float fillSpeed = 8f;

    private Coroutine currentRoutine;

    public void SetFill(float normalizedValue)
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(AnimateFill(normalizedValue));
    }

    private IEnumerator AnimateFill(float target)
    {
        float start = fillImage.fillAmount;

        while (Mathf.Abs(start - target) > 0.001f)
        {
            start = Mathf.Lerp(start, target, Time.deltaTime * fillSpeed);
            fillImage.fillAmount = start;
            yield return null;
        }

        fillImage.fillAmount = target;
    }
}