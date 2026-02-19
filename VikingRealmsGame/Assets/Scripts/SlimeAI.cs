using UnityEngine;

public class SlimeAI : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public float chaseSpeed = 3f;
    public float detectionRange = 5f;

    [Header("Combat Settings")]
    public int damage = 1;
    public float attackCooldown = 1f;

    [Header("References")]
    public Transform player;
    public Rigidbody2D rb;

    private Vector2 patrolDirection;
    private float patrolTimer;
    private float nextAttackTime;

    private enum State { Idle, Patrol, Chase }
    private State currentState = State.Patrol;

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        patrolDirection = GetRandomDirection();
        patrolTimer = Random.Range(1f, 3f);
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // State switching
        if (distanceToPlayer <= detectionRange)
            currentState = State.Chase;
        else if (currentState == State.Chase && distanceToPlayer > detectionRange)
            currentState = State.Patrol;

        switch (currentState)
        {
            case State.Patrol:
                Patrol();
                break;

            case State.Chase:
                ChasePlayer();
                break;
        }
    }

    void Patrol()
    {
        patrolTimer -= Time.deltaTime;

        // Move in a random direction
        rb.velocity = patrolDirection * moveSpeed;

        // Pick a new direction occasionally
        if (patrolTimer <= 0)
        {
            patrolDirection = GetRandomDirection();
            patrolTimer = Random.Range(1f, 3f);
        }
    }

    void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * chaseSpeed;
    }

    Vector2 GetRandomDirection()
    {
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (Time.time >= nextAttackTime)
            {
                // Call your player damage script here
                // Example: collision.collider.GetComponent<PlayerHealth>().TakeDamage(damage);

                nextAttackTime = Time.time + attackCooldown;
            }
        }
    }
}

