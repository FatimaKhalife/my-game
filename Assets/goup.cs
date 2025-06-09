using UnityEngine;

public class MoveOnTouch : MonoBehaviour
{
    public float moveSpeed = 2f;
    private bool isMoving = false;

    void Update()
    {
        if (isMoving)
        {
            // Move upward each frame
            transform.Translate(Vector2.up * moveSpeed * Time.deltaTime);
        }
    }

    // When the player touches the object
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isMoving = true;
        }
    }

    // When colliding with something other than the player
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player"))
        {
            isMoving = false;
        }
    }
}
