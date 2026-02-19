using UnityEngine;

public class SimpleMovment : MonoBehaviour
{
    public float speed = 5;
    public Rigidbody2D rb;
    public Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Flip only horizontally
        if (horizontal > 0 && transform.localScale.x < 0 ||
            horizontal < 0 && transform.localScale.x > 0)
        {
            FlipHorizontal();
        }

        // Send signed values to Blend Tree
        anim.SetFloat("MoveX", horizontal);
        anim.SetFloat("MoveY", vertical);

        bool isMoving = horizontal != 0 || vertical != 0;
        anim.SetBool("IsMoving", isMoving);

        rb.linearVelocity = new Vector2(horizontal, vertical) * speed;
    }

    void FlipHorizontal()
    {
        transform.localScale = new Vector3(
            transform.localScale.x * -1,
            transform.localScale.y,
            transform.localScale.z
        );
    }
}