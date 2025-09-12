using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnockback : MonoBehaviour
{
    public float damagePercent = 0f; 
    public float knockbackMultiplier = 0.1f; 
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void TakeHit(float baseKnockback, Vector2 attackDirection)
    {
        damagePercent += 10f; 

        float knockbackForce = baseKnockback + (damagePercent * knockbackMultiplier);

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
