using UnityEngine;
using UnityEngine.UI;

public class HealthColorGradient : MonoBehaviour
{
    [SerializeField] private Image fillImage;

    [Header("Colors")]
    public Color full = new Color(0.2f, 0.9f, 0.2f);
    public Color mid = new Color(1f, 0.8f, 0.2f);
    public Color low = new Color(1f, 0.4f, 0.1f);
    public Color critical = new Color(1f, 0.1f, 0.1f);

    public void UpdateColor(float normalized)
    {
        if (normalized > 0.7f)
            fillImage.color = Color.Lerp(mid, full, (normalized - 0.7f) / 0.3f);
        else if (normalized > 0.4f)
            fillImage.color = Color.Lerp(low, mid, (normalized - 0.4f) / 0.3f);
        else if (normalized > 0.2f)
            fillImage.color = Color.Lerp(critical, low, (normalized - 0.2f) / 0.2f);
        else
            fillImage.color = critical;
    }
}