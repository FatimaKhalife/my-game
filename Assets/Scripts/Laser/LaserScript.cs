using UnityEngine;

public class LaserScript : MonoBehaviour
{
    [Header("Laser pieces")]
    public GameObject laserStart;
    public GameObject laserMiddle;
    public GameObject laserEnd;

    private GameObject start;
    private GameObject middle;
    private GameObject end;

    void Update()
    {
        // Create the laser start from the prefab
        if (start == null)
        {
            start = Instantiate(laserStart, transform);
            start.transform.localPosition = Vector2.zero;
        }

        // Create the laser middle from the prefab
        if (middle == null)
        {
            middle = Instantiate(laserMiddle, transform);
            middle.transform.localPosition = Vector2.zero;
        }

        // Create the laser end from the prefab
        if (end == null)
        {
            end = Instantiate(laserEnd, transform);
            end.transform.localPosition = Vector2.zero;
        }

        float maxLaserSize = 20f;
        Vector2 laserDirection = transform.right;

        // Perform Raycast
        RaycastHit2D hit = Physics2D.Raycast(transform.position, laserDirection, maxLaserSize);

        // Determine the laser size based on hit
        float currentLaserSize = hit.collider ? Vector2.Distance(hit.point, transform.position) : maxLaserSize;

        // Adjust middle piece scale to match laser length
        middle.transform.localScale = new Vector2(currentLaserSize, middle.transform.localScale.y);

        // Position the middle piece correctly
        middle.transform.localPosition = new Vector2(currentLaserSize / 2f, 0);

        // Position the end piece at the end of the laser
        end.transform.localPosition = new Vector2(currentLaserSize, 0);
    }
}
