using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine;

namespace WorldTime
{
    [RequireComponent(typeof(Light2D))]
    public class WorldLight : MonoBehaviour
    {
        public float duration = 5f;

        [SerializeField] private Gradient gradient;
        [Header("Sun Elements (Sunrays/Sunspots)")]
        [SerializeField] private Light2D[] sunLights;
        [Header("Particle Effects")]
        [SerializeField] private ParticleSystem leafParticles; // Drag your leaf particle system here

        private Light2D _light;
        private float _startTime;

        private void Awake()
        {
            _light = GetComponent<Light2D>();
            _startTime = Time.time;
        }

        // Update is called once per frame
        void Update()
        {
            // Calculate the time elapsed since the start time
            var timeElapsed = Time.time - _startTime;
            // Calculate the percentage based on the sine of the time elapsed
            var percentage = Mathf.Sin(timeElapsed / duration * Mathf.PI * 2) * 0.5f + 0.5f;
            // Clamp the percentage to be between 0 and 1
            percentage = Mathf.Clamp01(percentage);

            _light.color = gradient.Evaluate(percentage);

            // Control sun lights (sunrays/sunspots) - visible during day, hidden at night
            if (sunLights != null && sunLights.Length > 0)
            {
                // Calculate sun intensity based on gradient timeline
                float sunIntensity;
                if (percentage < 0.192f) // Before sunrise (midnight to 19.2%)
                    sunIntensity = 0f;
                else if (percentage < 0.265f) // Sunrise (19.2% to 26.5%)
                    sunIntensity = (percentage - 0.192f) / (0.265f - 0.192f) * 0.9f; // Fade in to 90%
                else if (percentage < 0.55f) // Full day (26.5% to 55%)
                    sunIntensity = 0.9f; // Max brightness at 90%
                else if (percentage < 0.77f) // Fade to glimmer (55% to 77% at horizon)
                    sunIntensity = 0.9f - ((percentage - 0.55f) / (0.77f - 0.55f)) * 0.7f; // Fade to 20% glimmer
                else if (percentage < 0.88f) // Final fade out (77% to 88%)
                    sunIntensity = 0.2f * (1f - ((percentage - 0.77f) / (0.88f - 0.77f))); // Glimmer fades away
                else // Night (88% to 100%)
                    sunIntensity = 0f;
                
                foreach (var sunLight in sunLights)
                {
                    if (sunLight != null)
                    {
                        sunLight.intensity = sunIntensity;
                    }
                }
            }

            // Control leaf particles - only during day
            if (leafParticles != null)
            {
                // Start particles during day (19.2% to 77% when sun hits horizon), stop at sunset
                if (percentage >= 0.192f && percentage <= 0.77f)
                {
                    if (!leafParticles.isPlaying)
                        leafParticles.Play();
                }
                else
                {
                    if (leafParticles.isPlaying)
                        leafParticles.Stop();
                }
            }
        }
    }
}
