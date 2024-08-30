using UnityEngine;
using UnityEngine.Events;

public class InteractionMovementPlatform2D : MonoBehaviour
{
    public enum AutoMoveDirection
    {
        Up,
        Left,
        Right,
        Down
    }

    [Header("Movement Settings")]
    public float speed = 5f; // Speed of the character
    public bool freezeRotation;

    [Header("Jump Settings")]
    public float jumpForce = 5f; // Force applied when jumping
    public float gravityScale = 1f; // Gravity scale of the character
    public bool allowDoubleJump = false; // Allow double jumping
    public bool isGrounded = false;

    private Rigidbody2D rb;
    private Collider2D collider;
    private bool isJumping = false;
    private bool canDoubleJump = false;

    void Start()
    {
        // Add Rigidbody2D component if not attached
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
            rb = gameObject.AddComponent<Rigidbody2D>();

        // Add Collider2D component if not attached
        collider = GetComponent<Collider2D>();
        if (collider == null)
            Debug.LogWarning("Collider2D component not found. Please add a collider to the character.");

        if (LayerMask.NameToLayer("Ground") == -1)
        {
            Debug.LogError("No layer named 'Ground' found. Please create a layer named 'Ground' for proper ground detection.");
        }

        // Set gravity scale
        rb.gravityScale = gravityScale;
        if (freezeRotation)
        {
            rb.freezeRotation = freezeRotation;
        }
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(horizontalInput * speed, rb.velocity.y);
        // Move the character
        rb.velocity = movement;

        // Flip character if moving in the opposite direction
        if (horizontalInput != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(horizontalInput), 1, 1);
        }

        // Check if the character is grounded
        isGrounded = collider.IsTouchingLayers(LayerMask.GetMask("Ground"));

        // Reset double jump ability if grounded
        if (isGrounded)
        {
            canDoubleJump = true;
        }

        // Jumping logic
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                Jump();
            }
            else if (allowDoubleJump && canDoubleJump)
            {
                Jump();
                canDoubleJump = false;
            }
        }

    }

    public void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        isJumping = true;
    }
}
