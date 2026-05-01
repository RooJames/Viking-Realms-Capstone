using UnityEngine;
using UnityEngine.UI;

public class EnergyColorGradient : MonoBehaviour
{
    [SerializeField] private Image fillImage;

    public Color full = new Color(1f, 0.9f, 0.2f);
    public Color low = new Color(0.8f, 0.6f, 0.1f);

    public void UpdateColor(float normalized)
    {
        fillImage.color = Color.Lerp(low, full, normalized);
    }
}