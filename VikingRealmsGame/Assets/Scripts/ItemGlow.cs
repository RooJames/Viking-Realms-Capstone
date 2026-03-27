using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ItemGlow : MonoBehaviour
{
    public Light2D spriteLight;
    public Light2D pointLight;
    public float minIntensity;
    public float maxIntensity;
    public float pulseDuration;
    public float holdTime;

    void Update()
    {
        float cycleTime = Time.time % pulseDuration;
        float transitionTime = Mathf.Max(0.1f, (pulseDuration - holdTime) / 2f);
        float intensity;

        if (cycleTime < transitionTime)
        {
            // Fade up
            float t = cycleTime / transitionTime;
            intensity = Mathf.Lerp(minIntensity, maxIntensity, t);
        }
        else if (cycleTime < transitionTime + holdTime)
        {
            // Hold at max
            intensity = maxIntensity;
        }
        else
        {
            // Fade down
            float fadeStart = transitionTime + holdTime;
            float t = (cycleTime - fadeStart) / transitionTime;
            intensity = Mathf.Lerp(maxIntensity, minIntensity, t);
        }

        if (spriteLight != null)
            spriteLight.intensity = intensity;

        if (pointLight != null)
            pointLight.intensity = intensity;
    }
}