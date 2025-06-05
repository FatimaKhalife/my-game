using UnityEngine;

public class AutoLauncher : MonoBehaviour
{
    public GameObject projectilePrefab;     // Assign your projectile prefab
    public Transform firePoint;             // Assign the barrel tip
    public float fireForce = 10f;           // Speed of the projectile
    public float fireRate = 0.5f;           // Seconds between shots

    private float fireCooldown = 0f;

    void Update()
    {
        fireCooldown -= Time.deltaTime;
        if (fireCooldown <= 0f)
        {
            Fire();
            fireCooldown = fireRate;
        }
    }

    void Fire()
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.linearVelocity = firePoint.right * fireForce; // Fires in the direction the barrel is pointing
    }
}
