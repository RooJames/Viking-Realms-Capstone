using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("References")]
    public Animator anim;
    public GameObject meleeHitbox;

    private SimpleMovment move;

    [Header("Melee")]
    public float atkDuration = 0.25f;
    public float atkCooldown = 0.25f;

    private bool isAttacking = false;
    private float atkTimer = 0f;
    private float atkCooldownTimer = 0f;

    [Header("Ranged")]
    public Transform aim;
    public GameObject bulletPrefab;
    public float fireForce = 10f;
    public float shootCooldown = 0.25f;

    [Tooltip("Extra distance in front of aim to spawn bullet (prevents clipping).")]
    public float bulletSpawnOffset = 0.35f;

    private float shootTimer = 0f;

    void Start()
    {
        if (anim == null) anim = GetComponent<Animator>();
        move = GetComponent<SimpleMovment>();

        if (meleeHitbox != null)
            meleeHitbox.SetActive(false);

        shootTimer = shootCooldown;
        atkCooldownTimer = atkCooldown;
    }

    void Update()
    {
        shootTimer += Time.deltaTime;
        atkCooldownTimer += Time.deltaTime;

        if (isAttacking)
        {
            atkTimer += Time.deltaTime;
            if (atkTimer >= atkDuration)
                EndMelee();
        }

        // Melee
        if ((Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
            && atkCooldownTimer >= atkCooldown
            && !isAttacking)
        {
            StartMelee();
        }

        // Ranged
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetMouseButtonDown(1))
        {
            Shoot();
        }
    }

    void StartMelee()
    {
        isAttacking = true;
        atkTimer = 0f;
        atkCooldownTimer = 0f;

        if (meleeHitbox != null)
            meleeHitbox.SetActive(true);

        if (anim != null)
            anim.SetTrigger("Melee");
    }

    void EndMelee()
    {
        isAttacking = false;
        atkTimer = 0f;

        if (meleeHitbox != null)
            meleeHitbox.SetActive(false);
    }

    void Shoot()
    {
    if (shootTimer < shootCooldown) return;
    shootTimer = 0f;

    if (bulletPrefab == null || aim == null) return;

    // Get last movement direction
    Vector2 rawDir = (move != null) ? move.AimDirection : Vector2.right;

    // Clamp to horizontal only
    float x = rawDir.x;

    // If player was moving straight up/down,
    // fall back to flip direction
    if (Mathf.Abs(x) < 0.01f)
        x = (transform.localScale.x < 0) ? -1f : 1f;

    Vector2 dir = (x < 0) ? Vector2.left : Vector2.right;

    // Spawn slightly in front of player
    //float spawnOffset = 0.70f;
    Vector3 spawnPos = aim.position + (Vector3)(dir);

    GameObject b = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);

    Rigidbody2D rb = b.GetComponent<Rigidbody2D>();
    if (rb != null)
    {
        // FORCE travel direction
        rb.linearVelocity = dir * fireForce;
    }

    // Flip sprite ONLY when shooting LEFT
    SpriteRenderer sr = b.GetComponentInChildren<SpriteRenderer>();
    if (sr != null)
    {
        sr.flipX = (dir.x < 0);
    }

    Destroy(b, 2f);
    }
}