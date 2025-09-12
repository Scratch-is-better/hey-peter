using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    private Vector2 moveInput;
    private Rigidbody2D rb;
    private bool isGrounded;

    private Vector2 knockback;
    public float knockbackDecay = 5f;

    [Header("Combat")]
    public Transform attackPoint;
    public Vector3 rightAttackOffset;
    public Vector3 leftAttackOffset;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Mathf.Abs(rb.velocity.y) < 0.01f)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();

        if (moveInput.x > 0.01f)
            attackPoint.localPosition = rightAttackOffset;
        else if (moveInput.x < -0.01f)
            attackPoint.localPosition = leftAttackOffset;
    }
    
    void OnJump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f); 
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    void FixedUpdate()
    {
        Vector2 targetVelocity = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);

        // Apply knockback
        targetVelocity += knockback;
        rb.velocity = targetVelocity;

        // Knockback decay
        knockback = Vector2.Lerp(knockback, Vector2.zero, knockbackDecay * Time.fixedDeltaTime);
    }

    public void ApplyKnockback(Vector2 direction, float force)
    {
        knockback += direction * force;
    }
}
