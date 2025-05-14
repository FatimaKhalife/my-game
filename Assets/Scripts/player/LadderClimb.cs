using UnityEngine;

public class LadderClimb : MonoBehaviour
{
    public float climbSpeed = 3f; // Speed at which the player climbs
    public Transform ladderTop; // Reference to the top of the ladder (optional for boundary checks)

    private bool isClimbing = false; // Flag to check if the player is climbing
    private bool isNearLadder = false; // Flag to check if the player is near the ladder
    private bool isJumping = false; // Flag to check if the player is jumping
    private bool isGrounded = true; // Flag to check if the player is on the ground
    private Rigidbody2D rb; // Reference to the player's Rigidbody2D (assuming 2D)
    private Animator animator; // Reference to the player's Animator
    private float verticalInput; // Input for vertical movement (up or down)

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
        animator = GetComponent<Animator>(); // Get the Animator component
    }

    private void Update()
    {
        // Handle jumping only if the player is NOT climbing
        if (Input.GetKeyDown(KeyCode.Space) && !isClimbing)
        {
            Jump();
        }

        // Only allow climbing when the player is near the ladder
        if (isNearLadder && !isJumping)
        {
            // Start climbing when player presses "W" or "Up Arrow"
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                StartClimbing();
            }

            // Allow vertical movement while climbing
            if (isClimbing)
            {
                ClimbLadder();
            }
        }

        // Ensure the player can jump only if not climbing (or grounded)
        if (isGrounded && !isClimbing)
        {
            animator.SetBool("Jump", false); // Reset jump animation if grounded
        }
    }

    // Detect when the player enters or exits the ladder area (using triggers)
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ladder")) // Ensure the ladder object has the "Ladder" tag
        {
            isNearLadder = true; // Player is near the ladder
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            isNearLadder = false; // Player is no longer near the ladder
            StopClimbing();
        }
    }

    // Start the climbing mechanic
    private void StartClimbing()
    {
        isClimbing = true;
        rb.gravityScale = 0; // Disable gravity while climbing
        rb.linearVelocity = Vector2.zero; // Stop any existing movement

        // Trigger climbing animation and prevent jumping animation
        animator.SetBool("IsClimbing", true);
        animator.SetBool("Jump", false); // Disable jumping animation while climbing
    }

    // Stop the climbing mechanic
    private void StopClimbing()
    {
        isClimbing = false;
        rb.gravityScale = 1; // Re-enable gravity when done climbing

        // Trigger idle animation
        animator.SetBool("IsClimbing", false);
    }

    // Handle climbing movement
    private void ClimbLadder()
    {
        verticalInput = Input.GetAxis("Vertical"); // Get vertical movement input

        // Apply vertical movement based on player input (up or down)
        if (verticalInput != 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, verticalInput * climbSpeed);

            // Adjust animation speed to match the player's movement (optional)
            animator.SetFloat("ClimbSpeed", Mathf.Abs(verticalInput)); // Adjust climb speed in the animation
        }
    }

    // Handle jumping mechanics
    private void Jump()
    {
        if (!isClimbing && isGrounded) // Allow jumping only when grounded and not climbing
        {
            isJumping = true; // Set jump state

            // Apply jump force (adjust as needed)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 5f); // Example jump force value

            // Trigger jump animation
            animator.SetBool("Jump", true);
        }
    }

    // Handle collision with the ground (to reset jumping state)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            isJumping = false; // Reset jumping state
            animator.SetBool("Jump", false); // Disable jump animation if grounded
        }
    }

    // Handle leaving the ground
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
