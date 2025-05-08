using UnityEngine;

public class PlayerThrowArrow : MonoBehaviour
{
    public GameObject arrowPrefab;
    public Transform arrowSpawnPointRight; // For facing right
    public Transform arrowSpawnPointLeft;  // For facing left
    public float arrowSpeed = 10f;
    public float arrowLifetime = 3f; // Time before arrow disappears

    private PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            bool facingRight = playerMovement.IsFacingRight;
            Vector2 direction = facingRight ? Vector2.right : Vector2.left;

            // Select the spawn point based on the direction the player is facing
            Transform selectedSpawnPoint = facingRight ? arrowSpawnPointRight : arrowSpawnPointLeft;

            GameObject arrow = Instantiate(arrowPrefab, selectedSpawnPoint.position, Quaternion.identity);
            Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
            rb.linearVelocity = direction * arrowSpeed;

            // Set the arrow rotation based on direction
            float angle = facingRight ? 0f : 180f;
            arrow.transform.rotation = Quaternion.Euler(0, angle, 0);

            // Destroy the arrow after the specified lifetime
            Destroy(arrow, arrowLifetime);
        }
    }
}
