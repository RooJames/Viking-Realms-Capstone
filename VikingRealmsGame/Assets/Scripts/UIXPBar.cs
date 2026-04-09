using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIXPBar : MonoBehaviour
{
    [SerializeField] private Image fillMask;
    [SerializeField] private TMP_Text levelText;

    private void OnEnable()
    {
        PlayerXP.OnXPChanged += UpdateXP;
        PlayerXP.OnLevelUp += UpdateLevel;
    }

    private void OnDisable()
    {
        PlayerXP.OnXPChanged -= UpdateXP;
        PlayerXP.OnLevelUp -= UpdateLevel;
    }

    private void UpdateXP(float current, float required)
    {
        fillMask.fillAmount = current / required;
    }

    private void UpdateLevel(int level)
    {
        levelText.text = level.ToString();
    }
}