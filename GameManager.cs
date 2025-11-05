using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int startingLives = 3;
    public TextMeshProUGUI livesText;
    public GameObject gameOverPanel;

    public GameObject player;
    public Transform respawnPoint;
    public float respawnDelay = 1.5f;

    public PlayerController playerControllerToDisable;
    public TextMeshProUGUI koText;
    public float koDuration = 0.6f;

    int currentLives;
    bool isRespawning;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        if (player) player.SetActive(true);
        if (koText) koText.gameObject.SetActive(false);
    }

    void Start()
    {
        currentLives = startingLives;
        UpdateLivesUI();
        if (gameOverPanel) gameOverPanel.SetActive(false);
        if (player) player.SetActive(true);
    }

    public void PlayerHitKillzone()
    {
        if (isRespawning) return;

        StartCoroutine(ShowKORoutine());

        currentLives--;
        UpdateLivesUI();

        if (currentLives <= 0)
        {
            StartCoroutine(GameOverRoutine());
        }
        else
        {
            StartCoroutine(RespawnRoutine());
        }
    }

    IEnumerator RespawnRoutine()
    {
        isRespawning = true;

        if (player) player.SetActive(false);

        yield return new WaitForSeconds(respawnDelay);

        if (player && respawnPoint)
        {
            player.transform.position = respawnPoint.position;
            var rb = player.GetComponent<Rigidbody2D>();
            if (rb) { rb.velocity = Vector2.zero; rb.angularVelocity = 0f; }
            Physics2D.SyncTransforms();
            player.SetActive(true);
        }

        isRespawning = false;

        // make sure KO text is off after respawn
        if (koText) koText.gameObject.SetActive(false);
        isRespawning = false;
    }

    IEnumerator GameOverRoutine()
    {
        if (player) player.SetActive(false);
        if (playerControllerToDisable) playerControllerToDisable.enabled = false;

        yield return new WaitForSeconds(0.25f);

        if (gameOverPanel) gameOverPanel.SetActive(true);
    }
    IEnumerator ShowKORoutine()
    {
        if (koText == null) yield break;

        // show for koDuration, then hide (Respawn/GameOver will also ensure it’s hidden)
        koText.gameObject.SetActive(true);
        yield return new WaitForSeconds(koDuration);
        koText.gameObject.SetActive(false);
    }
    void UpdateLivesUI()
    {
        if (livesText) livesText.text = "Lives: " + currentLives;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Menu"); 
    }
}
