using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnockback : MonoBehaviour
{
    public float damagePercent = 0f; // Starts at 0%
    public float knockbackMultiplier = 0.1f; // How much knockback increases per %
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void TakeHit(float baseKnockback, Vector2 attackDirection)
    {
        // Increase damage percentage
        damagePercent += 10f; // Each hit increases by 10% (you can adjust)

        // Calculate knockback strength
        float knockbackForce = baseKnockback + (damagePercent * knockbackMultiplier);

        // Apply knockback
        rb.AddForce(attackDirection.normalized * knockbackForce, ForceMode2D.Impulse);

        Debug.Log(name + " at " + damagePercent + "%, took knockback " + knockbackForce);
    }

    public void ResetPlayer()
    {
        damagePercent = 0f;
        transform.position = Vector3.zero;
        rb.velocity = Vector2.zero;
    }
}
