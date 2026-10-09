using UnityEngine;
using UnityEngine.InputSystem;

public class RightPaddle : MonoBehaviour
{
    Rigidbody2D rb;

    private float movement;

    [SerializeField]
    float speed = 2f;

     void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

     void FixedUpdate()
    {
        rb.linearVelocity = new Vector2 (rb.linearVelocity.x, movement * speed);
    }

    public void OnMoveRight(InputValue value)
    {
        movement = value.Get<float>();
    }
}
