using UnityEngine;

public class ToneGenerator : MonoBehaviour
{
    public float volume = 0.5f; // Max volume limit
    private AudioSource audioSource;
    private float sampleRate = 40000f;
    private bool isPlaying = false; // Track whether audio is playing
    private float frequency = 440f; // Initial frequency, default is 440Hz (A4)
    public float targetFrequency;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("No AudioSource found on " + gameObject.name);
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
        audioSource.loop = true; // Allow looping, but manually control it
        audioSource.volume = Mathf.Clamp(volume, 0f, 0.5f);
    }

    public void StartTone(float freq)
    {
        if (!isPlaying) // Only start if not already playing
        {
            isPlaying = true;
            audioSource.clip = AudioClip.Create("Tone", Mathf.FloorToInt(sampleRate * 5), 1, Mathf.FloorToInt(sampleRate), true, OnAudioRead);
            audioSource.Play();
        }

        ChangeFrequency(freq);

        // Check if the frequency is correct
        if (Mathf.Abs(freq - targetFrequency) < 1f)  // Allow a small range for accuracy
        {
            // Play a success sound or trigger a visual cue (e.g., door opening)
            Debug.Log("Frequency matched!");
            // Example: Play a success sound or unlock the door here
        }
    }

    public void StopTone()
    {
        if (isPlaying)
        {
            isPlaying = false;
            audioSource.Stop(); // Stop sound when the slider is released
        }
    }

    public void ChangeFrequency(float newFrequency)
    {
        frequency = newFrequency; // Update the frequency to play the new tone
    }

    // **New Method to Get the Current Frequency**
    public float GetFrequency()
    {
        return frequency;
    }

    private float phase = 0f;
    private void OnAudioRead(float[] data)
    {
        float increment = 2 * Mathf.PI * frequency / sampleRate;
        for (int i = 0; i < data.Length; i++)
        {
            phase += increment;
            if (phase > 2 * Mathf.PI) phase -= 2 * Mathf.PI;
            data[i] = Mathf.Sin(phase) * volume;
        }
    }


}
