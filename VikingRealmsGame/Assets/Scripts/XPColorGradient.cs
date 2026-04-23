using UnityEngine;
using UnityEngine.UI;

public class XPColorGradient : MonoBehaviour
{
    [SerializeField] private Image fillImage;

    [Header("Gradient Colors")]
    public Color darkBlue = new Color(0.05f, 0.1f, 0.3f);     // deep blue
    public Color midBlue = new Color(0.2f, 0.4f, 0.8f);       // soft blue
    public Color whiteBlue = new Color(0.8f, 0.9f, 1f);       // wispy blue-white

    public void UpdateColor(float normalized)
    {
        if (fillImage == null)
            return;

        // 3‑point gradient
        if (normalized < 0.5f)
        {
            float t = normalized / 0.5f;
            fillImage.color = Color.Lerp(darkBlue, midBlue, t);
        }
        else
        {
            float t = (normalized - 0.5f) / 0.5f;
            fillImage.color = Color.Lerp(midBlue, whiteBlue, t);
        }
    }
}