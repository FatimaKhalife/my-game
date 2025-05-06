using UnityEngine;

public class Laser : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public Transform laserhit; // The point where the laser ends
    public float maxDistance = 10f; // Maximum laser distance

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = true;
        lineRenderer.useWorldSpace = true;
        lineRenderer.positionCount = 2; // Ensure at least 2 positions
    }

    void Update()
    {
        // Cast the ray **downward** instead of upward
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, maxDistance);
        Vector2 endPosition;

        if (hit.collider != null) // Check if the ray hits something
        {
            endPosition = hit.point;
            laserhit.position = hit.point;
        }
        else
        {
            endPosition = (Vector2)transform.position + Vector2.down * maxDistance; // Extend laser downward
            laserhit.position = endPosition;
        }

        // Set LineRenderer positions
        lineRenderer.SetPosition(0, transform.position); // Laser starts from top
        lineRenderer.SetPosition(1, endPosition); // Laser ends below
    }
}
