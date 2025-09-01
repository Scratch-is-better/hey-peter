using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public int lightAttackDamage = 10;
    public int heavyAttackDamage = 25;
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

    // Called automatically by PlayerInput (Send Messages)
    void OnLightAttack()
    {
        if (Time.time >= nextLightTime)
        {
            LightAttack();
            nextLightTime = Time.time + attackCooldown;
        }
    }

    // Called automatically by PlayerInput (Send Messages)
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
        DoAttack(lightAttackDamage, 5f); // small knockback
        TrackCombo();
    }

    void HeavyAttack()
    {
        Debug.Log(name + " did HEAVY attack");
        DoAttack(heavyAttackDamage, 10f); // big knockback
        comboStep = 0; // reset combo
    }

    void DoAttack(int damage, float knockbackForce)
    {
        if (attackPoint == null) return;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            var health = enemy.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }

            Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>();
            if (enemyRb != null)
            {
                Vector2 direction = (enemy.transform.position - transform.position).normalized;
                enemyRb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
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