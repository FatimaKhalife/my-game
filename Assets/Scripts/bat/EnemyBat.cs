using UnityEngine;

public class EnemyBat : MonoBehaviour
{
    public Transform firePoint;
    public GameObject wavePrefab;
    public float shootCooldown = 2f;
    public float detectionRange = 8f;
    public Transform player;

    private float shootTimer;

    void Update()
    {
        if (Vector2.Distance(transform.position, player.position) <= detectionRange)
        {
            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0f)
            {
                ShootWave();
                shootTimer = shootCooldown;
            }
        }
    }

    void ShootWave()
    {
        GameObject wave = Instantiate(wavePrefab, firePoint.position, Quaternion.identity);
        Vector2 direction = (player.position - transform.position).normalized;
        wave.GetComponent<WaveProjectile>().SetDirection(direction);
    }
}
