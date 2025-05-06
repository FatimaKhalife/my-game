using UnityEngine;

public class SmoothRandomFloat : MonoBehaviour
{
    public float movementSpeed = 1f;       // How fast it moves
    public float movementRange = 1.5f;     // How far from the origin it can move

    private Vector3 startPos;
    private float seedX;
    private float seedY;

    void Start()
    {
        startPos = transform.position;
        seedX = Random.Range(0f, 100f);
        seedY = Random.Range(0f, 100f);
    }

    void Update()
    {
        float x = Mathf.PerlinNoise(seedX, Time.time * movementSpeed);
        float y = Mathf.PerlinNoise(seedY, Time.time * movementSpeed);

        // Convert from [0, 1] range to [-1, 1] range
        x = (x - 0.5f) * 2f;
        y = (y - 0.5f) * 2f;

        // Apply movement
        Vector3 offset = new Vector3(x, y, 0) * movementRange;
        transform.position = startPos + offset;
    }
}
