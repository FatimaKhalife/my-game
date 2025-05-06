using UnityEngine;
using UnityEngine.UI; // Required for UI components

public class FrequencyBar : MonoBehaviour
{
    public Image fillImage; // Assign the fill Image in Inspector
    public float maxWidth = 5f; // Max width of the fill Image (in pixels)
    private float currentWidth = 0f;

    void Start()
    {
        // Set initial width to 0
        SetFillWidth(0);
    }

    public void SetFrequency(float frequency)
    {
        // Map frequency to width (e.g., 0-1 frequency to 0-maxWidth)
        currentWidth = Mathf.Clamp(frequency, 0f, 1f) * maxWidth;
        SetFillWidth(currentWidth);
    }

    private void SetFillWidth(float width)
    {
        // Update the width of the fill Image
        RectTransform fillRect = fillImage.rectTransform;
        fillRect.sizeDelta = new Vector2(width, fillRect.sizeDelta.y);
    }
}