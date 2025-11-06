using System.Collections;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private float fallDelay = 1f;
    [SerializeField] private float respawnDelay = 2f;
    [SerializeField] private float warnDuration = 0.35f;
    [SerializeField] private int warnFlashes = 3;       
    [SerializeField] private float shakeMagnitude = 0.06f;
    private bool falling = false;
    private bool armed = false;

    [SerializeField] private Rigidbody2D rb;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private Color baseColor = Color.white;

    private void Reset()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        if (rb != null) rb.bodyType = RigidbodyType2D.Kinematic;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (falling || armed) return;
        if (!collision.transform.CompareTag("Player")) return;

        StartCoroutine(StartFall());
    }

    private IEnumerator StartFall()
    {
        falling = true;
        armed = true;

        // Do a short drop warning (shake)
        float warnTime = Mathf.Min(warnDuration, Mathf.Max(0.05f, fallDelay * 0.5f));
        yield return WarningRoutine(warnTime);

        // Wait the remainder (if any) before actually dropping
        float remainder = Mathf.Max(0f, fallDelay - warnTime);
        if (remainder > 0f) yield return new WaitForSeconds(remainder);

        // Drop
        rb.bodyType = RigidbodyType2D.Dynamic;
        yield return new WaitForSeconds(respawnDelay);

        // Reset back to start
        ResetPlatform();
    }
    private IEnumerator WarningRoutine(float duration)
    {
        int steps = Mathf.Max(1, warnFlashes * 2);
        float step = duration / steps;

        // shake without drifting
        Vector3 originalLocalPos = transform.localPosition;

        for (int i = 0; i < steps; i++)
        {
            // tiny shake during each step
            float t = 0f;
            while (t < step)
            {
                t += Time.deltaTime;
                Vector2 offset = Random.insideUnitCircle * shakeMagnitude;
                transform.localPosition = originalLocalPos + new Vector3(offset.x, offset.y, 0f);
                yield return null;
            }
            // snap back to avoid drift
            transform.localPosition = originalLocalPos;
        }
    }

    private void ResetPlatform()
    {
        // make kinematic again and clear motion
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;

        // teleport back to start safely
        transform.position = startPosition;
        transform.rotation = startRotation;
        Physics2D.SyncTransforms();

        // allow re-trigger
        falling = false;
        armed = false;
    }
}
