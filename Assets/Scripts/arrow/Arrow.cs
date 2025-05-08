using UnityEngine;

public class StickyArrow : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool stuck = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!stuck && collision.collider.CompareTag("wall"))
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Static; // Stops arrow movement
            stuck = true;
            Debug.Log("Arrow stuck to wall.");

            // Optional: change layer if needed
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }
}
