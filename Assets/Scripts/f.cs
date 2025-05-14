using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    public float amplitude = 0.5f;     // How far it moves up and down
    public float frequency = 1f;       // How fast it moves

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        Vector3 newPos = startPos;
        newPos.y += Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = newPos;
    }
}
