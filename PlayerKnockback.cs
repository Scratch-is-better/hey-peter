using UnityEngine;

public class PlayerKnockback : MonoBehaviour
{
    public float damagePercent = 0f;
    public float knockbackMultiplier = 0.08f; // how much knockback scales per % damage
    private PlayerController controller;
    private PlayerAudio playerAudio;

    void Start()
    {
        controller = GetComponent<PlayerController>();
        playerAudio = GetComponent<PlayerAudio>();
    }

    public void TakeHit(float baseKnockback, Vector2 attackerPosition)
    {
        damagePercent += baseKnockback * 2f;

        float totalKnockback = baseKnockback * (1f + (damagePercent * knockbackMultiplier));

        Vector2 rawDir = ((Vector2)transform.position - attackerPosition).normalized;
        rawDir.y = Mathf.Clamp(rawDir.y, -0.1f, 0.1f);   
        rawDir = rawDir.normalized;                         
        controller?.ApplyKnockback(rawDir, totalKnockback);

        Debug.Log($"{gameObject.name} took knockback {totalKnockback:F2} | Damage%: {damagePercent:F1}");
        playerAudio?.PlayHit();
    }

    public void ResetPlayer()
    {
        damagePercent = 0f;
        controller?.ApplyKnockback(Vector2.zero, 0f);
        transform.position = Vector2.zero;
    }
}