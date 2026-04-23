using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BarEnergyShimmer : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private float shimmerSpeed = 3f;

    private Coroutine shimmerRoutine;

    public void SetShimmer(bool active)
    {
        if (active)
        {
            if (shimmerRoutine == null)
                shimmerRoutine = StartCoroutine(Shimmer());
        }
        else
        {
            if (shimmerRoutine != null)
                StopCoroutine(shimmerRoutine);

            shimmerRoutine = null;
        }
    }

    private IEnumerator Shimmer()
    {
        while (true)
        {
            float t = (Mathf.Sin(Time.time * shimmerSpeed) + 1f) / 2f;
            fillImage.color = Color.Lerp(fillImage.color, fillImage.color * 1.2f, t * 0.1f);
            yield return null;
        }
    }
}