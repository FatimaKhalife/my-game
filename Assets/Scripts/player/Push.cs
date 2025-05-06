using UnityEngine;
using System.Collections;

public class playerpush : MonoBehaviour
{
    public float distance = 1f;
    public LayerMask boxMask;

    private GameObject box;
    private bool isFacingRight = true; // Track player's direction

    void Start()
    {
        Physics2D.queriesStartInColliders = false;
    }

    void Update()
    {
        // Determine direction of the Raycast based on player facing direction
        Vector2 rayDirection = isFacingRight ? Vector2.right : Vector2.left;

        // Perform the Raycast in the correct direction
        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, distance, boxMask);

        // Debugging (optional, shows Raycast in Scene view)
        Debug.DrawRay(transform.position, rayDirection * distance, Color.yellow);

        // Handle picking up the box
        if (hit.collider != null && hit.collider.gameObject.CompareTag("pushable") && Input.GetKeyDown(KeyCode.E))
        {
            box = hit.collider.gameObject;

            FixedJoint2D joint = box.GetComponent<FixedJoint2D>();
            BoxMovement boxMovement = box.GetComponent<BoxMovement>();

            if (joint != null && boxMovement != null)
            {
                joint.connectedBody = this.GetComponent<Rigidbody2D>();
                joint.enabled = true;
                boxMovement.beingPushed = true;
            }
        }

        // Handle releasing the box
        else if (Input.GetKeyUp(KeyCode.E) && box != null)
        {
            FixedJoint2D joint = box.GetComponent<FixedJoint2D>();
            BoxMovement boxMovement = box.GetComponent<BoxMovement>();

            if (joint != null) joint.enabled = false;
            if (boxMovement != null)
            {
                boxMovement.beingPushed = false;

                // Reset the box's velocity to zero
                Rigidbody2D boxRigidbody = box.GetComponent<Rigidbody2D>();
                if (boxRigidbody != null)
                {
                    boxRigidbody.linearVelocity = Vector2.zero; // Stop all movement
                }
            }

            box = null; // Reset reference to avoid further issues
        }

        // Flip direction based on movement input
        float moveInput = Input.GetAxis("Horizontal");
        if (moveInput > 0)
            isFacingRight = true;
        else if (moveInput < 0)
            isFacingRight = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector2 rayDirection = isFacingRight ? Vector2.right : Vector2.left;
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + rayDirection * distance);
    }
}
