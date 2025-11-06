using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [Header("Attack Settings")]
    public float lightKnockback = 5f;
    public float attackRange = 0.6f;
    public float attackCooldown = 0.3f;
    public Transform attackPoint;
    public LayerMask enemyLayers;

    [Header("Charge Attack")]
    public float maxChargeTime = 2f;
    public float minChargeKnockback = 8f;
    public float maxChargeKnockback = 25f;
    private float currentCharge = 0f;
    private bool isCharging = false;

    [Header("Combo System")]
    public float comboResetTime = 0.5f;
    private int comboStep = 0;
    private float lastAttackTime;

    private float nextLightTime = 0f;

    private PlayerAudio playerAudio;

    void Start()
    {
        playerAudio = GetComponent<PlayerAudio>();
    }

    void Update()
    {
        if (isCharging)
            currentCharge += Time.deltaTime;
    }

    void OnLightAttack()
    {
        if (Time.time >= nextLightTime)
        {
            LightAttack();
            nextLightTime = Time.time + attackCooldown;
        }
    }

    void OnHeavyAttack(InputValue value)
    {
        if (value.isPressed)
        {
            isCharging = true;
            currentCharge = 0f;
            playerAudio?.PlayCharge();
            Debug.Log("Started charging...");
        }
        else if (isCharging)
        {
            isCharging = false;

            float chargeRatio = Mathf.Clamp01(currentCharge / maxChargeTime);
            float finalKnockback = Mathf.Lerp(minChargeKnockback, maxChargeKnockback, chargeRatio);
            Debug.Log($"Charge attack released with power {finalKnockback:F1}");

            DoAttack(finalKnockback);
            playerAudio?.PlayAttack();

            currentCharge = 0f;
        }
    }

    void LightAttack()
    {
        Debug.Log(name + " performed LIGHT attack");
        DoAttack(lightKnockback);
        playerAudio?.PlayAttack();
        TrackCombo();
    }

    void DoAttack(float knockbackForce)
    {
        if (attackPoint == null) return;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        bool connected = false;
        foreach (Collider2D enemy in hitEnemies)
        {
            PlayerKnockback kb = enemy.GetComponent<PlayerKnockback>();
            if (kb != null)
            {
                kb.TakeHit(knockbackForce, transform.position);
                playerAudio?.PlayHit();
                connected = true;
            }
        }
        if (connected)
            GetComponent<PlayerAttackFreeze>().OnAttackLanded();
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