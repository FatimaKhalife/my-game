using UnityEngine;
using UnityEngine.UI;

public class ColorChanger : MonoBehaviour
{
    public static ColorChanger Instance { get; private set; }
    public Camera mainCamera;
    public Color[] colors;
    private int currentColorIndex = -1;
    public GameObject colorPalette;
    public Image[] colorButtons;

    private bool isPaletteVisible = false;
    private Color selectedColor;
    private Image hoveredButton = null;

    private float clickDuration = 0f;
    private const float longClickThreshold = 0.5f;

    public Color initialBackgroundColor = Color.gray;

    private bool isPlayerInsideInvisiblePlatform = false; // Track if the player is inside an invisible platform

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        mainCamera.backgroundColor = initialBackgroundColor;
        colorPalette.SetActive(false);
    }

    void Update()
    {
        // If the player is inside an invisible platform, block the palette from showing
        if (isPlayerInsideInvisiblePlatform)
        {
            colorPalette.SetActive(false);
            return;
        }

        if (Input.GetMouseButton(0))
        {
            clickDuration += Time.deltaTime;
            ShowPalette(true);
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (clickDuration >= longClickThreshold)
            {
                ApplySelectedColor();
            }
            ShowPalette(false);
            clickDuration = 0f;
        }

        if (isPaletteVisible)
        {
            Vector3 mousePos = Input.mousePosition;

            for (int i = 0; i < colorButtons.Length; i++)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(colorButtons[i].rectTransform, mousePos))
                {
                    selectedColor = colors[i];
                    currentColorIndex = i;
                    HoverEffect(colorButtons[i]);
                    break;
                }
                else
                {
                    ResetHoverEffect(colorButtons[i]);
                }
            }
        }
    }

    public void ShowPalette(bool show)
    {
        isPaletteVisible = show;
        colorPalette.SetActive(show);
    }

    private void ApplySelectedColor()
    {
        if (selectedColor != null)
        {
            mainCamera.backgroundColor = selectedColor;
        }

        HandleObjectVisibility();
    }

    private void HandleObjectVisibility()
    {
        var platforms = FindObjectsByType<DisappearingPlatform>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var platform in platforms)
        {
            platform.CheckColor();
        }
    }

    private void HoverEffect(Image image)
    {
        if (hoveredButton != image)
        {
            if (hoveredButton != null)
                ResetHoverEffect(hoveredButton);

            hoveredButton = image;
            image.transform.localScale = new Vector3(1.2f, 1.2f, 1);
        }
    }

    private void ResetHoverEffect(Image image)
    {
        image.transform.localScale = Vector3.one;
    }

    public int GetCurrentColorIndex()
    {
        return currentColorIndex;
    }

    // Method to update the status of the invisible platform interaction
    public void SetPlayerInsideInvisiblePlatform(bool isInside)
    {
        isPlayerInsideInvisiblePlatform = isInside;
    }
}
