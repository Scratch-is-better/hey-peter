using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public int maxJumps = 2;
    private int jumpCount = 0;

    [Header("Crouch Settings")]
    public float crouchSpeedMultiplier = 0.5f;
    private bool isCrouching = false;

    [Header("Attack Point")]
    public Transform attackPoint;
    public Vector3 rightAttackOffset;
    public Vector3 leftAttackOffset;

    private Vector2 moveInput;
    private Rigidbody2D rb;
    private bool isGrounded;
    private Vector2 knockback;
    public float knockbackDecay = 5f;

    private PlayerAudio playerAudio;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAudio = GetComponent<PlayerAudio>();
    }

    void Update()
    {
  
        isGrounded = Mathf.Abs(rb.velocity.y) < 0.01f;

        if (isGrounded)
            jumpCount = 0; // reset jumps when grounded
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();

        // Flip attack point
        if (moveInput.x > 0.01f)
            attackPoint.localPosition = rightAttackOffset;
        else if (moveInput.x < -0.01f)
            attackPoint.localPosition = leftAttackOffset;
    }

    void OnJump()
    {
        if (jumpCount < maxJumps)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpCount++;
            playerAudio?.PlayJump();
        }
    }

    void OnCrouch(InputValue value)
    {
        isCrouching = value.isPressed;
        moveSpeed = isCrouching ? 5f * crouchSpeedMultiplier : 5f;
        transform.localScale = new Vector3(1f, isCrouching ? 0.7f : 1f, 1f);
    }

    void FixedUpdate()
    {
        Vector2 targetVelocity = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);
        targetVelocity += knockback; // apply knockback
        rb.velocity = targetVelocity;

        // Knockback decay
        knockback = Vector2.Lerp(knockback, Vector2.zero, knockbackDecay * Time.fixedDeltaTime);
    }

    public void ApplyKnockback(Vector2 direction, float force)
    {
        knockback += direction * force;
    }
}
