using UnityEngine;

public class GroundBreak : MonoBehaviour
{
    private Animator animator;
    private Collider2D groundCollider;
    private bool triggered = false;

    public float breakDelay = 1.0f; // Time after animation ends to disable collider

    void Start()
    {
        animator = GetComponent<Animator>();
        groundCollider = GetComponent<Collider2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!triggered && collision.collider.CompareTag("Player"))
        {
            triggered = true;
            animator.SetTrigger("Break"); // Trigger animation
            StartCoroutine(BreakAndDisable());
        }
    }

    private System.Collections.IEnumerator BreakAndDisable()
    {
        // Wait for the animation length (adjust if needed)
        yield return new WaitForSeconds(breakDelay);
        groundCollider.enabled = false;
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject); // Optional: remove the broken ground
    }
}
