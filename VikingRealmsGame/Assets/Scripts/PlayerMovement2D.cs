using UnityEngine;

public class PlayerMovement2D : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector3 move = new Vector3(x, y, 0f).normalized;
        transform.position += move * speed * Time.deltaTime;
    }
}