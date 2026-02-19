using UnityEngine;

public class EnemyHealthBarVisibility : MonoBehaviour
{
    public Transform player;
    public Transform enemy;
    public float showDistance = 5f;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        float distance = Vector2.Distance(player.position, enemy.position);

        if (distance <= showDistance)
            canvasGroup.alpha = 1f;   // show
        else
            canvasGroup.alpha = 0f;   // hide
    }
}
