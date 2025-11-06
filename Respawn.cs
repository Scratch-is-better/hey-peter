using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private void Start()
    {
        if (player) player.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Only react to the actual player root
        if (!player) return;
        if (other.transform.root.gameObject != player) return;
        if (other.isTrigger) return;

        // Hand off to GameManager. Do NOT respawn here.
        if (GameManager.Instance != null)
        {
            GameManager.Instance.PlayerHitKillzone();
        }
    }
}
