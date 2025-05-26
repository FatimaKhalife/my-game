using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifetime = 2f;

    void Start()
    {
        Destroy(gameObject, lifetime); // Destroy after a while to avoid clutter
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        
        Destroy(gameObject);
    }
}
