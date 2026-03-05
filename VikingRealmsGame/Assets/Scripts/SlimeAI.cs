using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SlimeAI : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public float chaseSpeed = 3f;
    public float detectionRange = 5f;

    [Header("Combat Settings")]
    public int damage = 1;
    public float attackCooldown = 1f;

    [Tooltip("Distance from slime to player where an attack should trigger (reliable even without collisions).")]
    public float attackRange = 0.8f;

    [Header("Patrol Settings")]
    public float minPatrolTime = 1f;
    public float maxPatrolTime = 3f;

    [Header("References")]
    public Transform player;

    private Rigidbody2D rb;
    private Animator animator;

    private Vector2 patrolDirection;
    private float patrolTimer;
    private float nextAttackTime;

    private enum State { Patrol, Chase }
    private State currentState = State.Patrol;

    // ✅ Match these EXACTLY to your Animator parameters (case-sensitive)
    private const string P_IS_MOVING = "IsMoving";   // change to "isMoving" if needed
    private const string P_IS_CHASING = "isChasing"; // optional; add this bool or remove the line in Update()
    private const string P_MOVEX = "moveX";
    private const string P_MOVEY = "moveY";
    private const string T_ATTACK = "attack";

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
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
        if (player == null)
        {
            rb.linearVelocity = Vector2.zero;
            UpdateAnimator(Vector2.zero);
            return;
        }

        float dist = Vector2.Distance(transform.position, player.position);
        currentState = (dist <= detectionRange) ? State.Chase : State.Patrol;

        if (animator != null)
            animator.SetBool(P_IS_CHASING, currentState == State.Chase);
    }

    void FixedUpdate()
    {
        if (player == null)
        {
            rb.linearVelocity = Vector2.zero;
            UpdateAnimator(Vector2.zero);
            return;
        }

        float distToPlayer = Vector2.Distance(rb.position, player.position);
        bool canAttackNow = distToPlayer <= attackRange && Time.time >= nextAttackTime;

        if (canAttackNow)
        {
            rb.linearVelocity = Vector2.zero;
            UpdateAnimator(Vector2.zero);

            if (animator != null)
            {
                // Face the player for directional attack
                Vector2 toPlayer = (Vector2)player.position - rb.position;
                if (toPlayer.sqrMagnitude > 0.0001f)
                {
                    Vector2 dir = toPlayer.normalized;
                    animator.SetFloat(P_MOVEX, dir.x);
                    animator.SetFloat(P_MOVEY, dir.y);
                }

                animator.SetTrigger(T_ATTACK);
            }

            nextAttackTime = Time.time + attackCooldown;
            return;
        }

        Vector2 velocity = currentState == State.Patrol ? PatrolMove() : ChaseMove(distToPlayer);

        rb.linearVelocity = velocity;
        UpdateAnimator(velocity);
    }

    Vector2 PatrolMove()
    {
        patrolTimer -= Time.fixedDeltaTime;
        if (patrolTimer <= 0f) PickNewPatrol();
        return patrolDirection * moveSpeed;
    }

    Vector2 ChaseMove(float distToPlayer)
    {
        if (distToPlayer <= attackRange)
            return Vector2.zero;

        Vector2 toPlayer = (Vector2)player.position - rb.position;
        if (toPlayer.sqrMagnitude < 0.0001f)
            return Vector2.zero;

        return toPlayer.normalized * chaseSpeed;
    }

    void PickNewPatrol()
    {
        patrolDirection = GetRandomDirection();
        patrolTimer = Random.Range(minPatrolTime, maxPatrolTime);
    }

    Vector2 GetRandomDirection()
    {
        Vector2 dir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        if (dir.sqrMagnitude < 0.01f) dir = Vector2.right;
        return dir.normalized;
    }

    void UpdateAnimator(Vector2 velocity)
    {
        if (animator == null) return;

        bool moving = velocity.sqrMagnitude > 0.01f;
        animator.SetBool(P_IS_MOVING, moving);

        if (moving)
        {
            Vector2 dir = velocity.normalized;
            animator.SetFloat(P_MOVEX, dir.x);
            animator.SetFloat(P_MOVEY, dir.y);
        }
    }
}