using UnityEngine;

public class EnemyBat : MonoBehaviour
{
    [Header("Attack Settings")]
    public Transform firePoint;
    public GameObject wavePrefab;
    public float shootCooldown = 2f;

    [Header("Movement Settings")]
    public float detectionRange = 8f;
    public float moveSpeed = 2f;
    public float stoppingDistance = 1.5f;

    [Header("Fly Away Settings")]
    public int maxHits = 10;
    public float flyAwaySpeed = 5f;
    public float destroyDistance = 20f;

    [Header("References")]
    public Transform player;

    private int hitCount = 0;
    private float shootTimer;
    private bool hasDetectedPlayer = false;
    private bool isFlyingAway = false;
    private Rigidbody2D rb;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isFlyingAway)
        {
            // Move upward when flying away
            transform.Translate(Vector2.up * flyAwaySpeed * Time.deltaTime);

            // Destroy when far enough from the player
            if (Vector2.Distance(transform.position, player.position) > destroyDistance)
            {
                Destroy(gameObject);
            }

            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (!hasDetectedPlayer && distanceToPlayer <= detectionRange)
        {
            hasDetectedPlayer = true;
        }

        if (hasDetectedPlayer)
        {
            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0f)
            {
                ShootWave();
                shootTimer = shootCooldown;
            }
        }
    }

    void FixedUpdate()
    {
        if (hasDetectedPlayer && !isFlyingAway)
        {
            FollowPlayer();
        }
    }

    void FollowPlayer()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer > stoppingDistance)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            Vector2 newPosition = (Vector2)transform.position + direction * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(newPosition);
        }
    }

    void ShootWave()
    {
        // Trigger attack animation
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        if (wavePrefab == null || player == null)
        {
            Debug.LogWarning("WavePrefab or Player reference missing.");
            return;
        }

        GameObject wave = Instantiate(wavePrefab, firePoint.position, Quaternion.identity);
        Vector2 direction = (player.position - transform.position).normalized;

        WaveProjectile waveScript = wave.GetComponent<WaveProjectile>();
        if (waveScript != null)
        {
            waveScript.SetDirection(direction);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerProjectile"))
        {
            hitCount++;
            Destroy(other.gameObject); // Destroy the projectile

            if (hitCount >= maxHits)
            {
                BeginFlyAway();
            }
        }
    }

    void BeginFlyAway()
    {
        isFlyingAway = true;
        hasDetectedPlayer = false;
        rb.linearVelocity = Vector2.zero;
       
        GetComponent<Collider2D>().enabled = false; // Optional: disable collision

    }
}
