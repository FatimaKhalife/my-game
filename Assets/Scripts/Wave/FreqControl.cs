using UnityEngine;
using UnityEngine.UI; // Required for UI components

public class FrequencyController : MonoBehaviour
{
    public FrequencyBar frequencyBar; // Assign in Inspector
    public float frequency = 0f; // Current frequency value
    public float increaseSpeed = 0.1f; // How fast the frequency increases/decreases

    public Button increaseButton; // Assign in Inspector
    public Button decreaseButton; // Assign in Inspector

    void Start()
    {
        // Add listeners for button clicks
        increaseButton.onClick.AddListener(IncreaseFrequency);
        decreaseButton.onClick.AddListener(DecreaseFrequency);
    }

    void Update()
    {
        // Clamp frequency between 0 and 1
        frequency = Mathf.Clamp(frequency, 0f, 1f);

        // Update the progress bar
        frequencyBar.SetFrequency(frequency);
    }

    public void IncreaseFrequency()
    {
        print(111);
        frequency += increaseSpeed * Time.deltaTime;
    }

    public void DecreaseFrequency()
    {
        frequency -= increaseSpeed * Time.deltaTime;
    }
}