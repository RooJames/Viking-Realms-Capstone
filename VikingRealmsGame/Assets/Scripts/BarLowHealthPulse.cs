using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BarLowHealthPulse : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private float pulseSpeed = 2f;

    private Coroutine pulseRoutine;

    public void SetPulsing(bool active)
    {
        if (active)
        {
            if (pulseRoutine == null)
                pulseRoutine = StartCoroutine(Pulse());
        }
        else
        {
            if (pulseRoutine != null)
                StopCoroutine(pulseRoutine);

            pulseRoutine = null;
            fillImage.color = fillImage.color; // reset
        }
    }

    private IEnumerator Pulse()
    {
        while (true)
        {
            float t = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;
            fillImage.color = Color.Lerp(Color.red, Color.white, t * 0.3f);
            yield return null;
        }
    }
}