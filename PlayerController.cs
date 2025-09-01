using UnityEngine;
using UnityEngine.InputSystem; 

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 moveInput;
    private Rigidbody2D rb;

    [Header("Combat")]
    public Transform attackPoint;       
    public Vector3 rightAttackOffset;   
    public Vector3 leftAttackOffset;   

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();

        if (moveInput.x > 0.01f)
            attackPoint.localPosition = rightAttackOffset;
        else if (moveInput.x < -0.01f)
            attackPoint.localPosition = leftAttackOffset;
    }

    void OnFire()
    {
        Debug.Log($"{gameObject.name} attacked!");
        //(Can add animation)
    }

    void FixedUpdate()
    {
        if (rb != null)
        {
            rb.velocity = moveInput * moveSpeed;
        }
    }
}