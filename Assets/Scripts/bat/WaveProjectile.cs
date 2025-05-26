using UnityEngine;

public class WaveProjectile : MonoBehaviour
{
    public float speed = 5f;
    public float lifeTime = 5f;
    private Vector2 moveDirection;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    public void SetDirection(Vector2 dir)
    {
        moveDirection = dir.normalized;
    }

    void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }

   void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Player"))
    {
        playerhealth player = other.GetComponent<playerhealth>();
        if (player != null)
        {
            player.TakeDamage(20); // or any amount you want
        }

        Destroy(gameObject); // destroy the wave after hitting the player
    }
    else if (!other.CompareTag("Enemy"))
    {
        Destroy(gameObject); // optional: if wave hits wall or other
    }
}

}
