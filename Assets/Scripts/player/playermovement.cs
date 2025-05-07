using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float carryingJumpForceModifier = 1.08f; // Modifier for jump force when carrying a box

    [Header("Ground Detection")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    private bool isGrounded;

    [Header("Box Pushing")]
    public float pushDistance = 1f;
    public LayerMask boxMask;
    private GameObject box;
    private bool isFacingRight = true;
    private bool isPushing = false;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    [Header("Jumping Settings")]
    public int maxJumpCount = 2; // Max consecutive jumps
    private int currentJumpCount = 0; // Tracks the current number of jumps
    private bool isOnBox = false; // Track if the player is standing on a box

    public SimpleWave echoWave; // Assuming SimpleWave exists and has ActivateWave and Done methods
    public bool waveDone = true;

    // Added for Moving Platforms
    private Transform currentPlatform = null;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Physics2D.queriesStartInColliders = false;
    }

    void Update()
    {
        HandleMovementInput();
        CheckGround();
        HandleJump();
        HandlePushing();
        UpdateAnimations();

        if (Input.GetKeyDown(KeyCode.Q) && waveDone)
        {
            // Spawn the wave at the player's position
            echoWave.ActivateWave(transform.position);
            waveDone = false;
        }
    }

    public void Done()
    {
        waveDone = true;
    }

    void HandleMovementInput()
    {
        float moveInput = Input.GetAxis("Horizontal");

        // If parented to a platform, movement input is relative to the platform
        // If not parented, movement is relative to the world
        if (currentPlatform != null)
        {
        
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        }
        else
        {
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        }


        // Flip sprite based on direction
        if (moveInput > 0)
            isFacingRight = true;
        else if (moveInput < 0)
            isFacingRight = false;

        spriteRenderer.flipX = !isFacingRight;
    }

    void CheckGround()
    {
        // Check if the player is grounded using the original logic
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 1f, groundLayer);

        // Reset jump count if grounded and not currently on a moving platform (handled by collision exit)
        if (isGrounded && currentPlatform == null)
        {
            currentJumpCount = 0; // Reset jump count when touching static ground
        }
        // If isGrounded becomes false while parented, OnCollisionExit2D will handle unparenting and jump reset
    }

    void HandleJump()
    {
        // Allow jumping if grounded or if current jump count is less than max
        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded || currentJumpCount < maxJumpCount))
        {
            Jump();
        }
    }

    void Jump()
    {
        // Apply jump force, modify if carrying a box
        float effectiveJumpForce = jumpForce;

        if (isPushing) // If the player is pushing a box
        {
            effectiveJumpForce *= carryingJumpForceModifier; // Reduce jump force
        }

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, effectiveJumpForce);
        currentJumpCount++;

        // If jumping while on a platform, unparent immediately
        if (currentPlatform != null)
        {
            transform.SetParent(null);
            currentPlatform = null;
            // Reset jump count when leaving a platform via jump
            currentJumpCount = 1; // Consider the jump off the platform as the first jump
        }

        isOnBox = false; // Player is no longer on the box when jumping
    }

    void HandlePushing()
    {
        Vector2 rayDirection = isFacingRight ? Vector2.right : Vector2.left;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, pushDistance, boxMask);

        if (hit.collider != null && hit.collider.CompareTag("pushable") && Input.GetKeyDown(KeyCode.E))
        {
            box = hit.collider.gameObject;
            FixedJoint2D joint = box.GetComponent<FixedJoint2D>();
            BoxMovement boxMovement = box.GetComponent<BoxMovement>(); // Assuming BoxMovement exists

            if (joint != null && boxMovement != null)
            {
                // Ensure the box is not parented to the moving platform before connecting
                if (box.transform.parent == currentPlatform)
                {
                    box.transform.SetParent(null);
                }

                joint.connectedBody = rb;
                joint.enabled = true;
                boxMovement.beingPushed = true;
                isPushing = true;

                // If pushing while on a platform, unparent the player from the platform
                if (currentPlatform != null)
                {
                    transform.SetParent(null);
                    currentPlatform = null;
                }
            }
        }
        else if (Input.GetKeyUp(KeyCode.E) && box != null)
        {
            ReleaseBox();
        }
    }

    void ReleaseBox()
    {
        FixedJoint2D joint = box.GetComponent<FixedJoint2D>();
        BoxMovement boxMovement = box.GetComponent<BoxMovement>(); // Assuming BoxMovement exists

        if (joint != null) joint.enabled = false;
        if (boxMovement != null)
        {
            boxMovement.beingPushed = false;
            Rigidbody2D boxRb = box.GetComponent<Rigidbody2D>();
            if (boxRb != null)
            {
                boxRb.linearVelocity = Vector2.zero; // Stop the box's movement
            }
        }

        // Re-parent the box to the platform if the player was on it when releasing
        // This might be complex depending on desired behavior. For now, leave it unparented.
        // if (currentPlatform != null)
        // {
        //     box.transform.SetParent(currentPlatform);
        // }

        box = null;
        isPushing = false;
    }

    void UpdateAnimations()
    {
        animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x)); // Set speed for running animation
        animator.SetBool("Jump", !isGrounded && !isOnBox); // Prevent jump animation if on the box
                                                           // Consider adding a specific animation state for being on a moving platform if needed
    }

    // Public method to reset jump count
    public void ResetJumpCount()
    {
        currentJumpCount = 0;
    }

    // Public method to track if the player is standing on a box
    public void SetIsOnBox(bool value)
    {
        isOnBox = value;
    }

    // Handle collision with other objects
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with a MovingPlatform and the player is grounded
        if (collision.gameObject.CompareTag("MovingPlatform") && isGrounded)
        {
            // Check if the collision is happening from below or the sides,
            // we only want to parent if landing on top.
            // We can do this by checking the contact points.
            foreach (ContactPoint2D contact in collision.contacts)
            {
                // A normal pointing upwards (close to Vector2.up) indicates collision from above
                if (Vector2.Dot(contact.normal, Vector2.up) > 0.5f) // Use a threshold like 0.5 to account for slopes
                {
                    transform.SetParent(collision.transform);
                    currentPlatform = collision.transform;
                    // Reset jump count when landing on a platform
                    currentJumpCount = 0;
                    break; // Found a top contact, no need to check others
                }
            }
        }
        // Also handle landing on a regular ground object to reset jump count
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") && currentPlatform == null)
        {
            currentJumpCount = 0;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Check if the exiting collision is the platform we were parented to
        if (collision.transform == currentPlatform)
        {
            transform.SetParent(null);
            currentPlatform = null;
            // Optionally reset jump count when leaving a platform (prevents infinite jumps by walking off)
            // currentJumpCount = 0; // Or set to 1 if you want to allow a double jump after walking off
        }
    }
}
