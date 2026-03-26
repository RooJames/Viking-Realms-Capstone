using UnityEngine;

public class RadialLayoutGroup : MonoBehaviour {
    public float radius = 100f;

    public void UpdateLayout() {
        int count = transform.childCount;
        if (count == 0) return;

        float angleStep = 360f / count;

        for (int i = 0; i < count; i++) {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector2 pos = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;

            RectTransform child = transform.GetChild(i).GetComponent<RectTransform>();
            child.anchoredPosition = pos;
        }
    }
}

