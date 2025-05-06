using UnityEngine;
using UnityEngine.UI;

public class FrequencySlider : MonoBehaviour
{
    public ToneGenerator toneGenerator;
    public Text frequencyDisplay;  // Reference to UI Text for displaying frequency
    public float targetFrequency = 440f;  // Set a target frequency (e.g., 440 Hz for A4)

    private Slider slider;

    void Start()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(OnSliderValueChanged);

        // Initialize display
        frequencyDisplay.text = "Current Frequency: " + toneGenerator.GetFrequency() + " Hz\nTarget: " + targetFrequency + " Hz";
    }

    // This method is called when the slider value changes
    public void OnSliderValueChanged(float value)
    {
        // Map slider value (0 to 1) to frequency range (20Hz to 2000Hz)
        float mappedFrequency = Mathf.Lerp(20f, 20000f, value);
        toneGenerator.StartTone(mappedFrequency);

        // Update frequency display
        frequencyDisplay.text = "Current Frequency: " + mappedFrequency.ToString("F2") + " Hz\nTarget: " + targetFrequency.ToString("F2") + " Hz";
    }

    // This method is called when the slider is released
    public void OnSliderReleased()
    {
        toneGenerator.StopTone();
    }
}
