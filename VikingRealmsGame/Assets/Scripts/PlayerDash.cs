using System;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    public static event Action<float, float> OnCooldownUpdate;

    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 14f;
    [SerializeField] private float dashDuration = 0.12f;
    [SerializeField] private float dashCooldown = 0.6f;

    private Rigidbody2D rb;
    private Vector2 dashDirection;
    private bool isDashing;
    private float dashTimer;
    private float cooldownTimer;

    private SimpleMovment movement; // FIXED

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<SimpleMovment>(); // FIXED
    }

    private void Update()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
            OnCooldownUpdate?.Invoke(cooldownTimer, dashCooldown);
        }

        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0f)
                EndDash();

            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) && cooldownTimer <= 0f)
            StartDash();
    }

    private void FixedUpdate()
    {
        if (isDashing)
            rb.linearVelocity = dashDirection * dashSpeed;
    }

    private void StartDash()
    {
        isDashing = true;
        dashTimer = dashDuration;

        Vector2 input = movement != null ? movement.MoveInput : Vector2.zero;
        dashDirection = input != Vector2.zero ? input.normalized : transform.right;

        OnCooldownUpdate?.Invoke(0f, dashCooldown);
    }

    private void EndDash()
    {
        isDashing = false;
        cooldownTimer = dashCooldown;

        OnCooldownUpdate?.Invoke(cooldownTimer, dashCooldown);
    }
}