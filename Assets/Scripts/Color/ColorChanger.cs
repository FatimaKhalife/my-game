using UnityEngine;
using UnityEngine.UI;
using System.Collections; // Required for Coroutines

public class ColorChanger : MonoBehaviour
{
    public static ColorChanger Instance { get; private set; }

    [Header("References")]
    public Camera mainCamera;
    public GameObject colorPalette; // The parent GameObject of your color buttons
    public Image[] colorButtons;
    public CanvasGroup paletteCanvasGroup; // Assign in Inspector or it will be auto-added

    [Header("Colors & Selection")]
    public Color[] colors;
    public Color initialBackgroundColor = Color.gray;
    private int currentColorIndex = -1; // Index of the color selected for application

    [Header("Transitions & Timings (Adjust for Speed)")]
    // Increased for faster button scaling
    public float buttonScaleSmoothSpeed = 1000f;
    // Increased for faster background color transition
    public float backgroundColorTransitionSpeed = 1000f;
    // Increased for faster palette fade
    public float paletteFadeSpeed = 1000f;
    public float longClickThreshold = 0.5f; // Time to hold for long click

    // Internal State
    private bool isPaletteVisible = false; // Logical state of palette visibility
    private float clickDuration = 0f;
    private bool isPlayerInsideInvisiblePlatform = false;
    private float[] targetScales; // For button hover scaling
    private Color targetCameraBackgroundColor; // Target for smooth background color lerp
    private Coroutine paletteFadeCoroutine;

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
            return;
        }

        if (colorPalette != null && paletteCanvasGroup == null)
        {
            paletteCanvasGroup = colorPalette.GetComponent<CanvasGroup>();
            if (paletteCanvasGroup == null)
            {
                paletteCanvasGroup = colorPalette.AddComponent<CanvasGroup>();
                Debug.LogWarning("ColorChanger: Automatically added CanvasGroup to the colorPalette GameObject. Please review its settings if needed.", colorPalette);
            }
        }
        else if (colorPalette == null)
        {
            Debug.LogError("ColorChanger: colorPalette GameObject is not assigned!", this);
        }
    }

    private void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.LogError("ColorChanger: Main Camera is not assigned and could not be found! Disabling script.", this);
                enabled = false;
                return;
            }
            Debug.LogWarning("ColorChanger: mainCamera was not assigned. Defaulted to Camera.main.", this);
        }

        mainCamera.backgroundColor = initialBackgroundColor;
        targetCameraBackgroundColor = initialBackgroundColor;

        if (paletteCanvasGroup != null)
        {
            paletteCanvasGroup.alpha = 0f;
            paletteCanvasGroup.interactable = false;
            paletteCanvasGroup.blocksRaycasts = false;
            if (colorPalette != null) colorPalette.SetActive(true); // Keep GameObject active
        }
        else if (colorPalette != null)
        {
            colorPalette.SetActive(false); // Fallback if no CanvasGroup
        }

        if (colorButtons != null && colorButtons.Length > 0)
        {
            targetScales = new float[colorButtons.Length];
            for (int i = 0; i < targetScales.Length; i++)
            {
                targetScales[i] = 1f;
            }
        }
        else
        {
            Debug.LogWarning("ColorChanger: colorButtons array is not assigned or is empty.", this);
            targetScales = new float[0]; // Initialize to empty to prevent null errors
        }
    }

    void Update()
    {
        if (isPlayerInsideInvisiblePlatform)
        {
            if (isPaletteVisible) ShowPalette(false);
            clickDuration = 0f;
            return;
        }

        HandleInput();

        if (isPaletteVisible)
        {
            UpdateButtonHoverStates();
        }

        SmoothlyChangeBackgroundColor();
    }

    private void LateUpdate()
    {
        if (isPaletteVisible || (paletteCanvasGroup != null && paletteCanvasGroup.alpha > 0.001f)) // Check against small alpha for fading out
        {
            SmoothButtonScaling();
        }
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clickDuration = 0f;
            if (!isPaletteVisible)
            {
                ShowPalette(true);
            }
        }

        if (Input.GetMouseButton(0))
        {
            clickDuration += Time.deltaTime;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (isPaletteVisible || clickDuration > 0f)
            {
                if (clickDuration >= longClickThreshold && currentColorIndex != -1)
                {
                    ApplySelectedColor();
                }
                if (isPaletteVisible)
                {
                    ShowPalette(false);
                }
            }
            clickDuration = 0f;
        }
    }

    private void SmoothlyChangeBackgroundColor()
    {
        if (mainCamera != null && mainCamera.backgroundColor != targetCameraBackgroundColor)
        {
            // For very fast changes, high speed value. If speed is extremely high, it's almost instant.
            mainCamera.backgroundColor = Color.Lerp(mainCamera.backgroundColor, targetCameraBackgroundColor, backgroundColorTransitionSpeed * Time.deltaTime);
        }
    }

    private void UpdateButtonHoverStates()
    {
        if (colorButtons == null || colorButtons.Length == 0 || targetScales.Length == 0) return;

        Vector3 mousePos = Input.mousePosition;
        int hoveredButtonIndex = -1;
        Canvas parentCanvas = (colorPalette != null) ? colorPalette.GetComponentInParent<Canvas>() : null;
        Camera uiCamera = null;
        if (parentCanvas && parentCanvas.renderMode != RenderMode.ScreenSpaceOverlay)
        {
            uiCamera = parentCanvas.worldCamera ?? mainCamera; // Use canvas camera or fallback
        }


        for (int i = 0; i < colorButtons.Length; i++)
        {
            if (colorButtons[i] == null || !colorButtons[i].gameObject.activeInHierarchy) continue;
            if (RectTransformUtility.RectangleContainsScreenPoint(colorButtons[i].rectTransform, mousePos, uiCamera))
            {
                hoveredButtonIndex = i;
                break;
            }
        }

        bool aButtonIsCurrentlyHovered = false;
        for (int i = 0; i < colorButtons.Length; i++)
        {
            if (colorButtons[i] == null) continue;
            // Ensure targetScales has been initialized and has the correct length
            if (i >= targetScales.Length) continue;

            if (i == hoveredButtonIndex)
            {
                targetScales[i] = 1.2f;
                currentColorIndex = i;
                aButtonIsCurrentlyHovered = true;
            }
            else
            {
                targetScales[i] = 1f;
            }
        }

        if (!aButtonIsCurrentlyHovered && isPaletteVisible)
        {
            // Optional: currentColorIndex = -1;
        }
    }

    private void SmoothButtonScaling()
    {
        if (colorButtons == null || targetScales.Length == 0) return;

        for (int i = 0; i < colorButtons.Length; i++)
        {
            if (colorButtons[i] == null || i >= targetScales.Length) continue;

            Image button = colorButtons[i];
            Vector3 currentButtonScale = button.transform.localScale;
            Vector3 targetButtonScaleVec = new Vector3(targetScales[i], targetScales[i], 1f);

            if (currentButtonScale != targetButtonScaleVec)
            {
                button.transform.localScale = Vector3.Lerp(
                    currentButtonScale,
                    targetButtonScaleVec,
                    buttonScaleSmoothSpeed * Time.deltaTime
                );
            }
        }
    }

    public void ShowPalette(bool show)
    {
        if (isPaletteVisible == show || colorPalette == null) return;

        if (paletteCanvasGroup == null && colorPalette != null)
        {
            colorPalette.SetActive(show);
            isPaletteVisible = show;
            if (!show) ResetButtonScalesAndTargets();
            return;
        }
        if (paletteCanvasGroup == null) return;

        isPaletteVisible = show;

        if (paletteFadeCoroutine != null)
        {
            StopCoroutine(paletteFadeCoroutine);
        }
        paletteFadeCoroutine = StartCoroutine(FadePaletteCoroutine(show));

        if (!show)
        {
            if (targetScales != null)
            {
                for (int i = 0; i < targetScales.Length; i++)
                {
                    targetScales[i] = 1f;
                }
            }
            // Optional: currentColorIndex = -1;
        }
    }

    private void ResetButtonScalesAndTargets()
    {
        if (colorButtons == null || targetScales == null) return;
        for (int i = 0; i < colorButtons.Length; i++)
        {
            if (colorButtons[i] != null)
            {
                colorButtons[i].transform.localScale = Vector3.one;
            }
            if (i < targetScales.Length)
            {
                targetScales[i] = 1f;
            }
        }
    }

    private IEnumerator FadePaletteCoroutine(bool fadeIn)
    {
        if (paletteCanvasGroup == null) yield break;

        float targetAlpha = fadeIn ? 1f : 0f;

        if (fadeIn)
        {
            if (colorPalette != null && !colorPalette.activeSelf) colorPalette.SetActive(true);
            paletteCanvasGroup.interactable = true;
            paletteCanvasGroup.blocksRaycasts = true;
        }

        float currentAlpha = paletteCanvasGroup.alpha;
        // For very fast changes, high speed value. Loop might run only once or twice.
        while (!Mathf.Approximately(currentAlpha, targetAlpha))
        {
            currentAlpha = Mathf.MoveTowards(currentAlpha, targetAlpha, paletteFadeSpeed * Time.deltaTime);
            paletteCanvasGroup.alpha = currentAlpha;
            yield return null;
        }
        paletteCanvasGroup.alpha = targetAlpha;

        if (!fadeIn)
        {
            paletteCanvasGroup.interactable = false;
            paletteCanvasGroup.blocksRaycasts = false;
        }
    }

    private void ApplySelectedColor()
    {
        if (colors != null && currentColorIndex != -1 && currentColorIndex < colors.Length)
        {
            targetCameraBackgroundColor = colors[currentColorIndex];
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

    public int GetCurrentColorIndex()
    {
        return currentColorIndex;
    }

    public void SetPlayerInsideInvisiblePlatform(bool isInside)
    {
        isPlayerInsideInvisiblePlatform = isInside;
        if (isInside && isPaletteVisible)
        {
            ShowPalette(false);
        }
    }
}