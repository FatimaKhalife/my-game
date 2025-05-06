using UnityEngine;

public class BoxJumpSurface : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement player = collision.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.ResetJumpCount(); // Reset the jump count
                player.SetIsOnBox(true); // Mark player as standing on the box
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement player = collision.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.SetIsOnBox(false); // Mark player as NOT on the box
            }
        }
    }
}
