using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float carryingJumpForceModifier = 1.08f;

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
    public int maxJumpCount = 2;
    private int currentJumpCount = 0;
    private bool isOnBox = false;

    public SimpleWave echoWave;
    public bool waveDone = true;

    private Transform currentPlatform = null;

    // Public getter for facing direction
    public bool IsFacingRight => isFacingRight;

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

        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if (moveInput > 0)
            isFacingRight = true;
        else if (moveInput < 0)
            isFacingRight = false;

        spriteRenderer.flipX = !isFacingRight;
    }

    void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 1f, groundLayer);

        if (isGrounded && currentPlatform == null)
        {
            currentJumpCount = 0;
        }
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded || currentJumpCount < maxJumpCount))
        {
            Jump();
        }
    }

    void Jump()
    {
        float effectiveJumpForce = jumpForce;

        if (isPushing)
        {
            effectiveJumpForce *= carryingJumpForceModifier;
        }

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, effectiveJumpForce);
        currentJumpCount++;

        if (currentPlatform != null)
        {
            transform.SetParent(null);
            currentPlatform = null;
            currentJumpCount = 1;
        }

        isOnBox = false;
    }

    void HandlePushing()
    {
        Vector2 rayDirection = isFacingRight ? Vector2.right : Vector2.left;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, pushDistance, boxMask);

        if (hit.collider != null && hit.collider.CompareTag("pushable") && Input.GetKeyDown(KeyCode.E))
        {
            box = hit.collider.gameObject;
            FixedJoint2D joint = box.GetComponent<FixedJoint2D>();
            BoxMovement boxMovement = box.GetComponent<BoxMovement>();

            if (joint != null && boxMovement != null)
            {
                if (box.transform.parent == currentPlatform)
                    box.transform.SetParent(null);

                joint.connectedBody = rb;
                joint.enabled = true;
                boxMovement.beingPushed = true;
                isPushing = true;

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
        BoxMovement boxMovement = box.GetComponent<BoxMovement>();

        if (joint != null) joint.enabled = false;

        if (boxMovement != null)
        {
            boxMovement.beingPushed = false;
            Rigidbody2D boxRb = box.GetComponent<Rigidbody2D>();
            if (boxRb != null)
            {
                boxRb.linearVelocity = Vector2.zero;
            }
        }

        box = null;
        isPushing = false;
    }

    void UpdateAnimations()
    {
        animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
        animator.SetBool("Jump", !isGrounded && !isOnBox);
    }

    public void ResetJumpCount()
    {
        currentJumpCount = 0;
    }

    public void SetIsOnBox(bool value)
    {
        isOnBox = value;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform") && isGrounded)
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (Vector2.Dot(contact.normal, Vector2.up) > 0.5f)
                {
                    transform.SetParent(collision.transform);
                    currentPlatform = collision.transform;
                    currentJumpCount = 0;
                    break;
                }
            }
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") && currentPlatform == null)
        {
            currentJumpCount = 0;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform == currentPlatform)
        {
            transform.SetParent(null);
            currentPlatform = null;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, 1.0f);
        }
    }

}
