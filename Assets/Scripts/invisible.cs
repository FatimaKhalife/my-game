using UnityEngine;

public class InvisiblePlatformTrigger : MonoBehaviour
{
    public DisappearingPlatform parentPlatform;

    void Start()
    {
        // Get reference to parent platform
        parentPlatform = GetComponentInParent<DisappearingPlatform>();
        if (parentPlatform == null)
        {
            Debug.LogError("InvisiblePlatformTrigger: Parent DisappearingPlatform not found!", this);
            enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            parentPlatform.PlayerEnteredInvisibleArea();
            print("enter");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            parentPlatform.PlayerExitedInvisibleArea();
            print("out");
        }
    }
}