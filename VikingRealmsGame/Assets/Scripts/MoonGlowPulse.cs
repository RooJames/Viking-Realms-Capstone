using UnityEngine;

public class MoonGlowPulse : MonoBehaviour
{
    [Header("Scale Settings")]
    public float scaleAmount = 0.03f;
    public float scaleSpeed = 0.6f;

    [Header("Alpha Settings")]
    [Range(0f, 1f)]
    public float minAlpha = 0.5f;

    [Range(0f, 1f)]
    public float maxAlpha = 0.8f;

    public float alphaSpeed = 0.6f;

    private Vector3 startScale;
    private SpriteRenderer sr;

    void Start()
    {
        startScale = transform.localScale;
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Scale pulse
        float scaleWave = Mathf.Sin(Time.time * scaleSpeed);
        transform.localScale = startScale * (1f + scaleWave * scaleAmount);

        // Alpha pulse
        float alphaWave = (Mathf.Sin(Time.time * alphaSpeed) + 1f) / 2f;
        float alpha = Mathf.Lerp(minAlpha, maxAlpha, alphaWave);

        Color c = sr.color;
        c.a = alpha;
        sr.color = c;
    }
}
