using UnityEngine;

public class FrequencyAnalyzer : MonoBehaviour
{
    public AudioSource audioSource;
    private float[] samples = new float[1024];  // Increased sample size
    private float frequency;
    private float sampleRate = 44100f;

    void Update()
    {
        if (audioSource.isPlaying)
        {
            audioSource.GetSpectrumData(samples, 0, FFTWindow.BlackmanHarris);  // Collect spectrum data
            frequency = GetDominantFrequency();
            Debug.Log("Dominant Frequency: " + frequency); // Output the frequency
        }
    }

    public float GetDominantFrequency()
    {
        float maxVal = 0;
        int maxIndex = 0;

        // Find the peak frequency
        for (int i = 0; i < samples.Length; i++)
        {
            if (samples[i] > maxVal)
            {
                maxVal = samples[i];
                maxIndex = i;
            }
        }

        // Calculate frequency (maxIndex is the bin index)
        float frequency = maxIndex * (sampleRate / 2) / samples.Length;
        return frequency;
    }
}
