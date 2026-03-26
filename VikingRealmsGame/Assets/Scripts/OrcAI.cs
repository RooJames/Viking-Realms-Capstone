using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class OrcAI : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2f;
    public float chaseSpeed = 3.5f;
    public float detectionRange = 6f;
    public float attackRange = 1.2f;
    public float runAttackRange = 2.5f;

    [Header("Combat")]
    public int damage = 2;
    public float attackCooldown = 1.5f;

    [Header("References")]
    public Transform player;

    private Rigidbody2D rb;
    private Animator animator;

    private float nextAttackTime;

    private enum State { Patrol, Chase, Attack }
    private State currentState;

    private Vector2 patrolDirection;
    private float patrolTimer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.freezeRotation = true;

        animator = GetComponent<Animator>();
    }

    void Start()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        PickNewPatrol();
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);

        if (dist <= detectionRange)
            currentState = State.Chase;
        else
            currentState = State.Patrol;
    }

    void FixedUpdate()
    {
        if (player == null) return;

        Vector2 velocity = Vector2.zero;

        switch (currentState)
        {
            case State.Patrol:
                velocity = PatrolMove();
                break;

            case State.Chase:
                velocity = ChaseMove();
                break;
        }

        rb.linearVelocity = velocity;
        UpdateAnimator(velocity);
    }

    Vector2 PatrolMove()
    {
        patrolTimer -= Time.fixedDeltaTime;

        if (patrolTimer <= 0f)
            PickNewPatrol();

        return patrolDirection * moveSpeed;
    }

    Vector2 ChaseMove()
    {
        Vector2 toPlayer = (Vector2)player.position - rb.position;
        float dist = toPlayer.magnitude;

        if (dist <= attackRange)
        {
            TryAttack(false); // regular attack
            return Vector2.zero;
        }

        if (dist <= runAttackRange && Time.time >= nextAttackTime)
        {
            TryAttack(true); // running attack
        }

        Vector2 dir = toPlayer.normalized;
        return dir * chaseSpeed;
    }

    void TryAttack(bool runningAttack)
    {
        if (Time.time < nextAttackTime) return;

        Vector2 dir = ((Vector2)player.position - rb.position).normalized;

        animator.SetFloat("MoveX", dir.x);
        animator.SetFloat("MoveY", dir.y);

        if (runningAttack)
            animator.SetTrigger("runAttack");
        else
            animator.SetTrigger("attack");

        nextAttackTime = Time.time + attackCooldown;
    }

    void PickNewPatrol()
    {
        patrolDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        patrolTimer = Random.Range(2f, 4f);
    }

    void UpdateAnimator(Vector2 velocity)
    {
        if (animator == null) return;

        bool moving = velocity.sqrMagnitude > 0.01f;

        animator.SetBool("IsMoving", moving);

        if (moving)
        {
            Vector2 dir = velocity.normalized;
            animator.SetFloat("MoveX", dir.x);
            animator.SetFloat("MoveY", dir.y);
        }
    }
}