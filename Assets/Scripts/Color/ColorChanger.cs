using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ColorChanger : MonoBehaviour
{
    public static ColorChanger Instance { get; private set; }

    [Header("References")]
    public Camera mainCamera;
    public GameObject colorPalette;
    public Image[] colorButtons;
    public CanvasGroup paletteCanvasGroup;

    [Header("Colors & Selection")]
    public Color[] colors;
    public Color initialBackgroundColor = Color.gray;
    private int currentColorIndex = -1;

    [Header("Transitions & Timings")]
    public float buttonScaleSmoothSpeed = 1000f;
    public float backgroundColorTransitionSpeed = 1000f;
    public float paletteFadeSpeed = 1000f;
    public float longClickThreshold = 0.5f;

    private bool isPaletteVisible = false;
    private float clickDuration = 0f;
    private bool isPlayerInsideInvisiblePlatform = false;
    private float[] targetScales;
    private Color targetCameraBackgroundColor;
    private Coroutine paletteFadeCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
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
            }
        }
    }

    private void Start()
    {
        InitializeColorChanger();
    }

    private void InitializeColorChanger()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("ColorChanger: Main Camera not found!", this);
            enabled = false;
            return;
        }

        mainCamera.backgroundColor = initialBackgroundColor;
        targetCameraBackgroundColor = initialBackgroundColor;

        if (paletteCanvasGroup != null)
        {
            paletteCanvasGroup.alpha = 0f;
            paletteCanvasGroup.interactable = false;
            paletteCanvasGroup.blocksRaycasts = false;
            colorPalette.SetActive(true);
        }

        if (colorButtons != null && colorButtons.Length > 0)
        {
            targetScales = new float[colorButtons.Length];
            for (int i = 0; i < targetScales.Length; i++)
            {
                targetScales[i] = 1f;
            }
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