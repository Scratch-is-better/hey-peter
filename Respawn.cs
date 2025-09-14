using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private float respawnDelay = 2f;
    // Start is called before the first frame update
    private float respawnTime;
    void Start()
    {
        player.SetActive(true);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.root.gameObject != player) return;
        if (other.isTrigger) return;
        player.SetActive(false);
        respawnTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.activeInHierarchy)
        {
            respawnTime += Time.deltaTime;
            if (respawnTime >= respawnDelay)
            {
                player.transform.position = respawnPoint.position;
                Physics2D.SyncTransforms();
                player.gameObject.SetActive(true);
                respawnTime = 0;
            }
        }
    }
}
