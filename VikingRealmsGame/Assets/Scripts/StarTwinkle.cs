using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class StarTwinkle : MonoBehaviour
{
    [Header("Alpha Range")]
    [Range(0f, 1f)] public float minAlpha = 0.0f;
    [Range(0f, 1f)] public float maxAlpha = 0.8f;

    [Header("Timing")]
    public float minCycleSeconds = 1.2f;
    public float maxCycleSeconds = 3.5f;

    [Header("Shape")]
    [Tooltip("Higher = snappier twinkle. 1 = smooth sine.")]
    public float intensity = 2.0f;

    private SpriteRenderer sr;
    private float speed;
    private float phase;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        // Randomize so they don't sync
        float cycle = Random.Range(minCycleSeconds, maxCycleSeconds);
        speed = (Mathf.PI * 2f) / cycle;
        phase = Random.Range(0f, Mathf.PI * 2f);
    }

    void Update()
    {
        // 0..1 wave
        float t = (Mathf.Sin(Time.time * speed + phase) + 1f) * 0.5f;

        // Make it more "sparkly" if intensity > 1
        t = Mathf.Pow(t, intensity);

        float a = Mathf.Lerp(minAlpha, maxAlpha, t);

        Color c = sr.color;
        c.a = a;
        sr.color = c;
    }
}