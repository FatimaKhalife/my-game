using UnityEngine;
using UnityEngine.UI;

public class DisappearingPlatform : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Collider2D platformCollider;
    public int platformColorIndex;

    private bool playerInside = false;
    public Text playerMessageText; // Assign this in the Inspector

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        platformCollider = GetComponent<Collider2D>();
        CheckColor(); // Initial check

        if (playerMessageText != null)
        {
            playerMessageText.gameObject.SetActive(false); // Start with message hidden
        }
        else
        {
            Debug.LogWarning("PlayerMessageText is not assigned on " + gameObject.name);
        }
    }

    public void CheckColor()
    {
        if (ColorChanger.Instance == null)
        {
            Debug.LogError("ColorChanger.Instance is null. Make sure a ColorChanger exists in the scene and is initialized.");
            return;
        }

        int currentColorIndex = ColorChanger.Instance.GetCurrentColorIndex();

        // if (currentColorIndex == -1) return; // Consider if you still need this check or if GetCurrentColorIndex always returns a valid index

        if (currentColorIndex == platformColorIndex)
        {
            spriteRenderer.enabled = false;
            platformCollider.isTrigger = true;
        }
        else
        {
            spriteRenderer.enabled = true;
            platformCollider.isTrigger = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            if (ColorChanger.Instance != null)
            {
                ColorChanger.Instance.SetPlayerInsideInvisiblePlatform(true);
            }
       
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            if (ColorChanger.Instance != null)
            {
                ColorChanger.Instance.SetPlayerInsideInvisiblePlatform(false);
            }
            // CheckColor(); // Re-evaluate if this call is needed.
           
        }
    }

    void Update()
    {
        if (playerMessageText == null) return; // Guard clause

        if (playerInside)
        {
            // Only show message if player is inside AND trying to change color AND the platform is currently invisible (and thus a trigger)
            bool isTryingToChangeColor = Input.GetMouseButton(0);
            bool platformIsCurrentlyInvisibleTrigger = platformCollider.isTrigger; // More direct check

            if (platformIsCurrentlyInvisibleTrigger && isTryingToChangeColor)
            {
                playerMessageText.gameObject.SetActive(true);
            }
            else
            {
                playerMessageText.gameObject.SetActive(false);
            }
        }
        else // Player is outside
        {
            if (playerMessageText.gameObject.activeSelf) // Only deactivate if it's currently active
            {
                playerMessageText.gameObject.SetActive(false);
            }
        }
    }
}