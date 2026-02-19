using UnityEngine;
using TMPro;

public class FloatingDamageText : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float lifetime = 1f;
    [SerializeField] private Vector3 moveDirection = new Vector3(0, 1, 0);

    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void Initialize(float damage)
    {
        text.text = Mathf.RoundToInt(damage).ToString();
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }
}
