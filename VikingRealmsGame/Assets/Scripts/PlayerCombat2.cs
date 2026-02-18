using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("References")]
    public Animator anim;
    public GameObject meleeHitbox;

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

    private float shootTimer = 0f;

    void Start()
    {
        // auto-find animator if you forgot to assign it
        if (anim == null)
            anim = GetComponent<Animator>();

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

        //  isAttacking so you don’t restart attack mid-swing
        if ((Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
            && atkCooldownTimer >= atkCooldown
            && !isAttacking)
        {
            StartMelee();
        }

        if (Input.GetKeyDown(KeyCode.Q) || Input.GetMouseButtonDown(1))
            Shoot();
    }

    void StartMelee()
    {
        isAttacking = true;
        atkTimer = 0f;
        atkCooldownTimer = 0f;

        if (meleeHitbox != null)
            meleeHitbox.SetActive(true);

        if (anim != null)
            anim.SetTrigger("attack");
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

        GameObject b = Instantiate(bulletPrefab, aim.position, aim.rotation);

        Rigidbody2D rb = b.GetComponent<Rigidbody2D>();
       if (rb != null)
        {
         Vector2 dir = (transform.localScale.x < 0) ? Vector2.left : Vector2.right;
         rb.AddForce(dir * fireForce, ForceMode2D.Impulse);
        }


        Destroy(b, 2f);
    }
}
