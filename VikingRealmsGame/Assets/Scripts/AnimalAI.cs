using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AnimalAI : MonoBehaviour
{
    public enum AnimalType { Fox, Hare, Deer, Boar }

    [Header("Animal Type")]
    public AnimalType animalType = AnimalType.Deer;

    [Header("Movement")]
    public float walkSpeed = 1.5f;
    public float runSpeed = 4f;
    public float roamRadius = 5f;
    public float idleTimeMin = 1.5f;
    public float idleTimeMax = 3.5f;
    public float roamMoveTime = 4f;

    [Header("Detection")]
    public float fleeRange = 5f;       // prey flee when player enters this range
    public float aggroRange = 7f;      // boar chase range after being attacked
    public float attackRange = 1.2f;   // boar melee range

    [Header("Combat (Boar Only)")]
    public int damage = 3;
    public float attackCooldown = 1.5f;

    [Header("Death")]
    public float deathAnimDuration = 1.5f; // seconds before object is destroyed

    [Header("Drops")]
    [Tooltip("Drop prefab spawned on death (Meat, Fur, etc). Leave empty for no drop.")]
    public GameObject meatPrefab;

    // ── private fields ──────────────────────────────────────────────────────
    private Rigidbody2D rb;
    private Animator animator;
    private Transform player;
    private Health health;

    private enum State { Idle, Roam, Flee, Chase, Attack, Dead }
    private State currentState = State.Idle;

    private Vector2 roamTarget;
    private float stateTimer;
    private float nextAttackTime;
    private bool hasBeenAttacked; // boar only – triggers aggro on first hit

    // Derived from animalType (set in Start)
    private bool fleeFromPlayer;
    private bool isAggressive;
    private bool dropsMeat;

    // ── Animator parameter names ─────────────────────────────────────────────
    // Make sure your Animator Controller has these exact parameters:
    //   Float  : moveX, moveY
    //   Bool   : IsMoving, IsRunning
    //   Trigger: hurt, die, attack   (attack only needed for Boar)
    private static readonly int ParamMoveX     = Animator.StringToHash("moveX");
    private static readonly int ParamMoveY     = Animator.StringToHash("moveY");
    private static readonly int ParamIsMoving  = Animator.StringToHash("IsMoving");
    private static readonly int ParamIsRunning = Animator.StringToHash("IsRunning");
    private static readonly int ParamHurt      = Animator.StringToHash("hurt");
    private static readonly int ParamDie       = Animator.StringToHash("die");
    private static readonly int ParamAttack    = Animator.StringToHash("attack");

    // ── Unity lifecycle ──────────────────────────────────────────────────────
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.freezeRotation = true;
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
    }

    void Start()
    {
        // Derive behaviour flags from animal type
        switch (animalType)
        {
            case AnimalType.Fox:
                fleeFromPlayer = false;
                isAggressive   = false;
                dropsMeat      = true;
                break;
            case AnimalType.Hare:
                fleeFromPlayer = true;
                isAggressive   = false;
                dropsMeat      = true;
                break;
            case AnimalType.Deer:
                fleeFromPlayer = true;
                isAggressive   = false;
                dropsMeat      = true;
                break;
            case AnimalType.Boar:
                fleeFromPlayer = false;
                isAggressive   = true;
                dropsMeat      = true;
                break;
        }

        // Find player
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;

        // Subscribe to Health events
        if (health != null)
        {
            health.OnDamaged.AddListener(HandleDamaged);
            health.OnDeath.AddListener(HandleDeath);
        }

        EnterIdle();
    }

    void OnDestroy()
    {
        if (health != null)
        {
            health.OnDamaged.RemoveListener(HandleDamaged);
            health.OnDeath.RemoveListener(HandleDeath);
        }
    }

    // ── Update / FixedUpdate ─────────────────────────────────────────────────
    void Update()
    {
        if (currentState == State.Dead || player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);

        switch (currentState)
        {
            case State.Idle:
            case State.Roam:
                if (fleeFromPlayer && dist <= fleeRange)
                    EnterFlee();
                else if (isAggressive && hasBeenAttacked && dist <= aggroRange)
                    EnterChase();
                else
                    TickRoam();
                break;

            case State.Flee:
                // Stop fleeing once the player is safely far away
                if (!fleeFromPlayer || dist > fleeRange * 1.5f)
                    EnterIdle();
                break;

            case State.Chase:
                if (dist <= attackRange)
                    currentState = State.Attack;
                else if (dist > aggroRange * 1.2f)
                {
                    hasBeenAttacked = false;
                    EnterIdle();
                }
                break;

            case State.Attack:
                if (dist > attackRange)
                    currentState = State.Chase;
                break;
        }
    }

    void FixedUpdate()
    {
        if (currentState == State.Dead)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 velocity = Vector2.zero;

        switch (currentState)
        {
            case State.Roam:    velocity = RoamVelocity();  break;
            case State.Flee:    velocity = FleeVelocity();  break;
            case State.Chase:   velocity = ChaseVelocity(); break;
            case State.Attack:  TryAttack(); break;
            // Idle: velocity stays zero
        }

        rb.linearVelocity = velocity;
        UpdateAnimator(velocity);
    }

    // ── State transitions ────────────────────────────────────────────────────
    void EnterIdle()
    {
        currentState = State.Idle;
        stateTimer = Random.Range(idleTimeMin, idleTimeMax);
    }

    void EnterRoam()
    {
        currentState = State.Roam;
        stateTimer = roamMoveTime;
        Vector2 offset = Random.insideUnitCircle * roamRadius;
        roamTarget = (Vector2)transform.position + offset;
    }

    void EnterFlee()  => currentState = State.Flee;
    void EnterChase() => currentState = State.Chase;

    // ── Roam tick (called from Update) ──────────────────────────────────────
    void TickRoam()
    {
        stateTimer -= Time.deltaTime;

        if (currentState == State.Idle)
        {
            if (stateTimer <= 0f) EnterRoam();
        }
        else // Roam
        {
            float distToTarget = Vector2.Distance(transform.position, roamTarget);
            if (distToTarget < 0.25f || stateTimer <= 0f)
                EnterIdle();
        }
    }

    // ── Velocity helpers ─────────────────────────────────────────────────────
    Vector2 RoamVelocity()
    {
        Vector2 dir = (roamTarget - (Vector2)transform.position).normalized;
        return dir * walkSpeed;
    }

    Vector2 FleeVelocity()
    {
        if (player == null) return Vector2.zero;
        Vector2 away = ((Vector2)transform.position - (Vector2)player.position).normalized;
        return away * runSpeed;
    }

    Vector2 ChaseVelocity()
    {
        if (player == null) return Vector2.zero;
        Vector2 toward = ((Vector2)player.position - (Vector2)transform.position).normalized;
        return toward * runSpeed;
    }

    // ── Boar attack ──────────────────────────────────────────────────────────
    void TryAttack()
    {
        if (Time.time < nextAttackTime || player == null) return;

        // Deal damage to player
        IDamageable target = player.GetComponent<IDamageable>();
        if (target != null && !target.IsDead)
            target.TakeDamage(damage);

        if (animator != null)
            animator.SetTrigger(ParamAttack);

        nextAttackTime = Time.time + attackCooldown;
    }

    // ── Animator ─────────────────────────────────────────────────────────────
    void UpdateAnimator(Vector2 velocity)
    {
        if (animator == null) return;

        bool moving  = velocity.sqrMagnitude > 0.01f;
        bool running = moving && (currentState == State.Flee || currentState == State.Chase);

        animator.SetBool(ParamIsMoving,  moving);
        animator.SetBool(ParamIsRunning, running);

        if (moving)
        {
            Vector2 dir = velocity.normalized;
            animator.SetFloat(ParamMoveX, dir.x);
            animator.SetFloat(ParamMoveY, dir.y);
        }
    }

    // ── Health event handlers ─────────────────────────────────────────────────
    void HandleDamaged(float amount)
    {
        if (currentState == State.Dead) return;

        if (animator != null)
            animator.SetTrigger(ParamHurt);

        if (animalType == AnimalType.Boar)
        {
            hasBeenAttacked = true;
            EnterChase();
        }
        else if (fleeFromPlayer)
        {
            // Prey animals immediately bolt when hit
            EnterFlee();
        }
    }

    void HandleDeath()
    {
        currentState = State.Dead;
        rb.linearVelocity = Vector2.zero;

        if (animator != null)
            animator.SetTrigger(ParamDie);

        // Drop meat before the object is destroyed
        if (dropsMeat && meatPrefab != null)
        {
            Vector2 dropOffset = Random.insideUnitCircle * 0.4f;
            Instantiate(meatPrefab, (Vector2)transform.position + dropOffset, Quaternion.identity);
        }

        // Destroy after the death animation finishes
        // NOTE: set destroyOnDeath = false on the Health component so Health
        // doesn't destroy the object at 0.1s – let this coroutine handle it.
        StartCoroutine(DestroyAfterDeathAnim());
    }

    IEnumerator DestroyAfterDeathAnim()
    {
        yield return new WaitForSeconds(deathAnimDuration);
        Destroy(gameObject);
    }

    // ── Editor gizmos ────────────────────────────────────────────────────────
    void OnDrawGizmosSelected()
    {
        if (fleeFromPlayer || animalType == AnimalType.Hare || animalType == AnimalType.Deer)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, fleeRange);
        }
        if (animalType == AnimalType.Boar)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, aggroRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}
