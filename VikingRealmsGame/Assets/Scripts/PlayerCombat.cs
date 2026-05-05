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

    [Header("Combat SFX")]
    public AudioSource actionAudioSource;
    public AudioClip meleeSfx;
    public AudioClip blastSfx;

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
        if (Input.GetKeyDown(KeyCode.E)
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

        if (GSController.sfxOn && actionAudioSource && meleeSfx)
        {
            actionAudioSource.PlayOneShot(meleeSfx);
        }

        // Determine attack direction from last movement
        Vector2 dir = (move != null) ? move.AimDirection : Vector2.right;

        // Snap to the closest cardinal/horizontal direction
        // (no left — right is mirrored via player scale flip)
        float ax = Mathf.Abs(dir.x);
        float ay = Mathf.Abs(dir.y);

        Vector2 snapDir;
        if (ay > ax)                              // up or down
            snapDir = (dir.y >= 0) ? Vector2.up : Vector2.down;
        else                                      // left or right
            snapDir = (dir.x >= 0) ? Vector2.right : Vector2.left;

        // Tell the animator which direction so the Blend Tree picks the right clip
        if (anim != null)
        {
            anim.SetFloat("LastMoveX", snapDir.x);
            anim.SetFloat("LastMoveY", snapDir.y);
            anim.SetBool("isAttacking", true);
            anim.SetTrigger("attack"); //melee
        }

        // Reposition hitbox to match attack direction
        if (meleeHitbox != null)
        {
            Transform hb = meleeHitbox.transform;
            if (snapDir == Vector2.up)
                hb.localPosition = new Vector3(0f, 0.3f, 0f);
            else if (snapDir == Vector2.down)
                hb.localPosition = new Vector3(0f, -0.3f, 0f);
            else  // right or left (sprite flip handles left)
                hb.localPosition = new Vector3(0.3f, 0f, 0f);

            meleeHitbox.SetActive(true);
        }
    }

    void EndMelee()
    {
        isAttacking = false;
        atkTimer = 0f;

        if (anim != null)
            anim.SetBool("isAttacking", false);

        if (meleeHitbox != null)
            meleeHitbox.SetActive(false);
    }

    void Shoot()
    {
    if (shootTimer < shootCooldown) return;
    shootTimer = 0f;

    if (bulletPrefab == null || aim == null) return;

    if (GSController.sfxOn && actionAudioSource && blastSfx)
    {
        actionAudioSource.PlayOneShot(blastSfx);
    }

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
    Vector3 spawnPos = transform.position + (Vector3)(dir * bulletSpawnOffset);
spawnPos.z = 0f;

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