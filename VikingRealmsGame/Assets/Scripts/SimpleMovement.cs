using UnityEngine;

public class SimpleMovment : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Animator anim;

    private Rigidbody2D rb;
    private float speedX;
    private float speedY;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // auto-find animator if you forgot to drag it in Inspector
        if (anim == null)
            anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        speedX = Input.GetAxisRaw("Horizontal");
        speedY = Input.GetAxisRaw("Vertical");

        // flip left/right
        if ((speedX > 0 && transform.localScale.x < 0) ||
            (speedX < 0 && transform.localScale.x > 0))
        {
            Flip();
        }

        // feed your animator params (keep your 0.1 thresholds in Animator transitions)
        if (anim != null)
        {
            anim.SetFloat("speedX", Mathf.Abs(speedX));
            anim.SetFloat("speedY", Mathf.Abs(speedY));
        }

        rb.linearVelocity = new Vector2(speedX * moveSpeed, speedY * moveSpeed);
    }

    void Flip()
    {
        transform.localScale = new Vector3(
            transform.localScale.x * -1,
            transform.localScale.y,
            transform.localScale.z
        );
    }
}


