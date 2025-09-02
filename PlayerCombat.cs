using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public float lightKnockback = 5f;
    public float heavyKnockback = 10f;
    public float attackRange = 0.6f;
    public float attackCooldown = 0.3f;
    public float heavyCooldown = 0.8f;
    public Transform attackPoint;
    public LayerMask enemyLayers;

    private float nextLightTime = 0f;
    private float nextHeavyTime = 0f;

    // Combo tracking
    private int comboStep = 0;
    private float comboResetTime = 0.5f;
    private float lastAttackTime;

    void OnLightAttack()
    {
        if (Time.time >= nextLightTime)
        {
            LightAttack();
            nextLightTime = Time.time + attackCooldown;
        }
    }

    void OnHeavyAttack()
    {
        if (Time.time >= nextHeavyTime)
        {
            HeavyAttack();
            nextHeavyTime = Time.time + heavyCooldown;
        }
    }

    void LightAttack()
    {
        Debug.Log(name + " did LIGHT attack");
        DoAttack(lightKnockback);
        TrackCombo();
    }

    void HeavyAttack()
    {
        Debug.Log(name + " did HEAVY attack");
        DoAttack(heavyKnockback);
        comboStep = 0; // reset combo
    }

    void DoAttack(float knockbackForce)
    {
        if (attackPoint == null) return;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            // apply knockback if the target has a PlayerController
            PlayerController controller = enemy.GetComponent<PlayerController>();
            if (controller != null)
            {
                Vector2 direction = (enemy.transform.position - transform.position).normalized;
                controller.ApplyKnockback(direction, knockbackForce);
                Debug.Log(enemy.name + " got knocked back with force " + knockbackForce);
            }
        }
    }

    void TrackCombo()
    {
        if (Time.time - lastAttackTime < comboResetTime)
        {
            comboStep++;
            if (comboStep == 2) Debug.Log("Combo Step 2!");
            if (comboStep == 3) Debug.Log("Combo Finisher!");
        }
        else
        {
            comboStep = 1;
        }

        lastAttackTime = Time.time;
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}