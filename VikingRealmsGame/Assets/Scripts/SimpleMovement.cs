using UnityEngine;

public class SimpleMovment : MonoBehaviour
{
    public float speed = 5f;
    public Rigidbody2D rb;
    public Animator anim;

    // Last direction moved/aimed (used for shooting)
    public Vector2 AimDirection { get; private set; } = Vector2.right;

    // NEW: Expose movement input for PlayerDash
    public Vector2 MoveInput { get; private set; }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (anim == null)
            anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        // Freeze movement while dialogue is open
        if (DialogueUI.Instance != null && DialogueUI.Instance.IsOpen)
        {
            rb.linearVelocity = Vector2.zero;
            if (anim != null) anim.SetBool("IsMoving", false);
            return;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector2 input = new Vector2(horizontal, vertical);

        // NEW: expose input to other scripts
        MoveInput = input;

        // Update aim direction ONLY when moving
        if (input.sqrMagnitude > 0.01f)
            AimDirection = input.normalized;

        // Flip only horizontally
        if ((horizontal > 0 && transform.localScale.x < 0) ||
            (horizontal < 0 && transform.localScale.x > 0))
        {
            FlipHorizontal();
        }

        // Animator params
        bool moving = input.sqrMagnitude > 0.01f;
        if (anim != null)
        {
            anim.SetFloat("MoveX", horizontal);
            anim.SetFloat("MoveY", vertical);
            anim.SetBool("IsMoving", moving);
        }

        // Movement
        rb.linearVelocity = input * speed;
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