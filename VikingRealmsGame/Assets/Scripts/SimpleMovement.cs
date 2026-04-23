using UnityEngine;

public class SimpleMovment : MonoBehaviour
{
    public float speed = 5f;
    public float exhaustedSpeed = 2f;
    public Rigidbody2D rb;
    public Animator anim;

    private PlayerStats stats;

    public Vector2 AimDirection { get; private set; } = Vector2.right;
    public Vector2 MoveInput { get; private set; }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<PlayerStats>();

        if (anim == null)
            anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector2 input = new Vector2(horizontal, vertical);
        MoveInput = input;

        if (input.sqrMagnitude > 0.01f)
            AimDirection = input.normalized;

        if ((horizontal > 0 && transform.localScale.x < 0) ||
            (horizontal < 0 && transform.localScale.x > 0))
        {
            FlipHorizontal();
        }

        if (anim != null)
        {
            anim.SetFloat("MoveX", horizontal);
            anim.SetFloat("MoveY", vertical);
            anim.SetBool("IsMoving", input.sqrMagnitude > 0.01f);
        }

        float currentSpeed = stats != null && stats.isExhausted ? exhaustedSpeed : speed;

        rb.linearVelocity = input.normalized * currentSpeed;
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