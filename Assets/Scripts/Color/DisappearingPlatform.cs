using UnityEngine;
using UnityEngine.UI;

public class DisappearingPlatform : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Collider2D platformCollider;
    public int platformColorIndex;

    private bool playerInside = false;
    public Text playerMessageText;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        platformCollider = GetComponent<Collider2D>();
        CheckColor();

        if (playerMessageText != null)
        {
            playerMessageText.gameObject.SetActive(false);
        }
    }

    public void CheckColor()
    {
        int currentColorIndex = ColorChanger.Instance.GetCurrentColorIndex();

        if (currentColorIndex == -1) return;

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
            ColorChanger.Instance.SetPlayerInsideInvisiblePlatform(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            ColorChanger.Instance.SetPlayerInsideInvisiblePlatform(false);
            CheckColor();
            playerMessageText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (playerInside)
        {
            bool isTryingToChangeColor = Input.GetMouseButton(0);
            playerMessageText.gameObject.SetActive(isTryingToChangeColor);
        }
    }
}