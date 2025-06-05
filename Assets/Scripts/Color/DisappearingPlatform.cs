using UnityEngine;
using UnityEngine.UI;

public class DisappearingPlatform : MonoBehaviour
{
    [Header("Components")]
    private SpriteRenderer spriteRenderer;
    private Collider2D platformCollider;

    [Header("Settings")]
    public int platformColorIndex;

    [Header("Invisible Trigger")]
    public GameObject invisibleTrigger; // Assign a child trigger in Inspector
    public Collider2D extraChildCollider; // Assign in Inspector

    [Header("UI")]
    public Text playerMessageText;

    private bool playerInside = false;
    private bool isInitialized = false;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        platformCollider = GetComponent<Collider2D>();
    }

    void Start()
    {
        Initialize();
        CheckColor();
    }

    private void Initialize()
    {
        if (isInitialized) return;

        if (invisibleTrigger != null)
            invisibleTrigger.SetActive(false);

        if (playerMessageText != null)
            playerMessageText.gameObject.SetActive(false);

        isInitialized = true;
    }

    public void ResetPlatform()
    {
        playerInside = false;
        Initialize();
        CheckColor(); // Re-check color to reset collision/visibility
    }

    public void CheckColor()
    {
        if (ColorChanger.Instance == null)
        {
            Debug.LogError("ColorChanger.Instance is null. Make sure a ColorChanger exists in the scene.");
            return;
        }

        int currentColorIndex = ColorChanger.Instance.GetCurrentColorIndex();
        bool isInvisible = (currentColorIndex == platformColorIndex);

        // Handle main object visibility/collision
        if (spriteRenderer != null)
            spriteRenderer.enabled = !isInvisible;

        UpdateCollisionBehavior(isInvisible);

        // Handle deadly children
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Deadly"))
            {
                if (child.TryGetComponent<SpriteRenderer>(out var childSprite))
                    childSprite.enabled = !isInvisible;

                if (child.TryGetComponent<Collider2D>(out var childCollider))
                    childCollider.enabled = !isInvisible;
            }
        }
    }

    private void UpdateCollisionBehavior(bool isInvisible)
    {
        // Handle main collider
        if (platformCollider != null)
        {
            // For "dis" tagged objects: Use trigger mode for pass-through behavior
            if (gameObject.CompareTag("dis"))
            {
                platformCollider.isTrigger = isInvisible;
            }
            // For regular platforms: Use collision ignoring
            else
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null && player.TryGetComponent<Collider2D>(out var playerCollider))
                {
                    Physics2D.IgnoreCollision(platformCollider, playerCollider, isInvisible);
                }
            }
        }

        // Handle extra child collider (if exists)
        if (extraChildCollider != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null && player.TryGetComponent<Collider2D>(out var playerCollider))
            {
                Physics2D.IgnoreCollision(extraChildCollider, playerCollider, isInvisible);
            }
        }

        // Handle invisible trigger
        if (invisibleTrigger != null)
            invisibleTrigger.SetActive(isInvisible);
    }

    // Called by the invisible trigger child
    public void PlayerEnteredInvisibleArea()
    {
        playerInside = true;
        if (ColorChanger.Instance != null)
        {
            ColorChanger.Instance.SetPlayerInsideInvisiblePlatform(true);
        }
    }

    public void PlayerExitedInvisibleArea()
    {
        playerInside = false;
        if (ColorChanger.Instance != null)
        {
            ColorChanger.Instance.SetPlayerInsideInvisiblePlatform(false);
        }
    }

    void Update()
    {
        if (playerMessageText == null) return;

        if (playerInside)
        {
            bool isTryingToChangeColor = Input.GetMouseButton(0);
            playerMessageText.gameObject.SetActive(isTryingToChangeColor);
        }
        else
        {
            playerMessageText.gameObject.SetActive(false);
        }
    }
}