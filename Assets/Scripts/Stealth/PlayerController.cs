using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    private Rigidbody2D rb;
    private Vector2 movementDirection;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        movementDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

    }

    void FixedUpdate()
    {
        rb.linearVelocity = movementDirection * speed;
    }
}
