using UnityEngine;
using TMPro;

public class UITalentPoints : MonoBehaviour
{
    [SerializeField] private GameObject container;
    [SerializeField] private TMP_Text countText;

    private void OnEnable()
    {
        PlayerTalents.OnPointsChanged += UpdatePoints;
    }

    private void OnDisable()
    {
        PlayerTalents.OnPointsChanged -= UpdatePoints;
    }

    private void UpdatePoints(int points)
    {
        container.SetActive(points > 0);
        countText.text = points.ToString();
    }
}