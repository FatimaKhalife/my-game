using UnityEngine;

public class FallingBall : MonoBehaviour
{
    public float speed = 2f;
    public float height = 3f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Ping-pong motion between start and start+height
        float newY = startPos.y + Mathf.PingPong(Time.time * speed, height);
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}
