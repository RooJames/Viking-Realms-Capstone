using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CampfireLight : MonoBehaviour
{
    [SerializeField] private Light2D campfireLight;
    [SerializeField] private float duration = 600f; // Should match WorldLight duration
    
    [Header("Light Settings")]
    [SerializeField] private float maxIntensity = 1f;
    [SerializeField] private float nightStartPercentage = 0.77f; // When sun hits horizon (77%)
    [SerializeField] private float nightEndPercentage = 0.192f; // When sunrise begins (19.2%)
    
    private float _startTime;
    
    void Awake()
    {
        _startTime = Time.time;
        
        if (campfireLight == null)
        {
            campfireLight = GetComponent<Light2D>();
        }
    }
    
    void Update()
    {
        // Calculate the time elapsed since the start time
        float timeElapsed = Time.time - _startTime;
        // Calculate the percentage based on the sine of the time elapsed
        float percentage = Mathf.Sin(timeElapsed / duration * Mathf.PI * 2) * 0.5f + 0.5f;
        percentage = Mathf.Clamp01(percentage);
        
        // Calculate light intensity based on time of day
        float lightIntensity;
        
        if (percentage < nightEndPercentage)
        {
            // Early morning to sunrise (0% to 19.2%) - fade out
            lightIntensity = (nightEndPercentage - percentage) / nightEndPercentage * maxIntensity;
        }
        else if (percentage < nightStartPercentage)
        {
            // Day time (19.2% to 77%) - off
            lightIntensity = 0f;
        }
        else
        {
            // Evening to midnight (77% to 100%) - fade in
            lightIntensity = (percentage - nightStartPercentage) / (1f - nightStartPercentage) * maxIntensity;
        }
        
        if (campfireLight != null)
        {
            campfireLight.intensity = lightIntensity;
        }
    }
}
