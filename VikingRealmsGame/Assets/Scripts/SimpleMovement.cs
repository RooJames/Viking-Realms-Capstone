using UnityEngine;

public class SimpleMovment : MonoBehaviour
{
    public float speed = 5;
    public Rigidbody2D rb;
    public Animator anim;

    private KnockbackReceiver knockback;   // NEW

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        knockback = GetComponent<KnockbackReceiver>();   // NEW

        if (anim == null)
            anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        // NEW: stop movement during knockback / hit-stun
        if (knockback != null && knockback.InHitStun)
            return;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if ((horizontal > 0 && transform.localScale.x < 0) ||
            (horizontal < 0 && transform.localScale.x > 0))
        {
            FlipHorizontal();
        }

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