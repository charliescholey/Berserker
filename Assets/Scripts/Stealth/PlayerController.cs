using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 6f;
    private Rigidbody2D rigidBody;
    private Vector2 movementDirection;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movementDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

    }

    void FixedUpdate()
    {
        rigidBody.linearVelocity = movementDirection * playerSpeed;
    }
}
