using UnityEngine;

public class SimpleMovment :MonoBehaviour
{
    public float moveSpeed = 5f;
    public Animator anim;
    public int facingDirection = 1;

    private Rigidbody2D rb;
    private float speedX;
    private float speedY;

    

    void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    void FixedUpdate ()
    {

	speedX = Input.GetAxisRaw("Horizontal");
        speedY = Input.GetAxisRaw("Vertical");

        if(speedX > 0 && transform.localScale.x <0 || speedX < 0 && transform.localScale.x > 0 )
	{
          Flip();
		
	}

        anim.SetFloat("speedX", Mathf.Abs(speedX));
        anim.SetFloat("speedY", Mathf.Abs(speedY));

        rb.linearVelocity = new Vector2(speedX * moveSpeed, speedY * moveSpeed);
    }


    void Flip()
    {
	facingDirection *= -1;
	transform.localScale = new Vector3 (transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);

    }
}

