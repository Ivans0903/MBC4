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
    public float dashSpeed = 20f; // Speed during dash
    public float dashDuration = 0.2f; // Duration of the dash
    public bool freezeRotation;

    [Header("Jump Settings")]
    public float jumpForce = 5f; // Force applied when jumping
    public float gravityScale = 1f; // Gravity scale of the character
    public bool allowDoubleJump = false; // Allow double jumping
    public bool isGrounded = false;

    [Header("Animation Settings")]
    public UnityEvent IdleEvent;
    public UnityEvent RunEvent;
    public UnityEvent JumpEvent;
    public UnityEvent DashEvent; // New event for dash animation

    private Rigidbody2D rb;
    private Animator animator;
    private Collider2D collider;
    private bool isJumping = false;
    private bool canDoubleJump = false;
    private bool isDashing = false;
    private float dashTime;

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

        // Cache Animator component if exists
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        // Trigger Dash with X key
        if (Input.GetKeyDown(KeyCode.X) && !isDashing)
        {
            Dash(horizontalInput);
        }

        // Normal movement if not dashing
        if (!isDashing)
        {
            Vector2 movement = new Vector2(horizontalInput * speed, rb.velocity.y);
            rb.velocity = movement;
        }

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

        // Dash timer logic
        if (isDashing)
        {
            dashTime -= Time.deltaTime;
            if (dashTime <= 0)
            {
                isDashing = false;
            }
        }

        // Animation handling
        if (animator != null)
        {
            if (isGrounded && Mathf.Abs(horizontalInput) < 0.01f && !isDashing)
            {
                IdleEvent?.Invoke();
            }
            else if (isGrounded && isDashing)
            {
                DashEvent?.Invoke(); // Trigger dash animation when dashing
            }
            else if (isGrounded && !isDashing)
            {
                if (speed > 1)
                {
                    RunEvent?.Invoke();
                }
       
            }
            else if (!isGrounded)
            {
                JumpEvent?.Invoke();
            }
        }
    }

    public void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        isJumping = true;
    }

    public void Dash(float direction)
    {
        isDashing = true;
        dashTime = dashDuration;
        rb.velocity = new Vector2(Mathf.Sign(direction) * dashSpeed, rb.velocity.y);
    }
}