using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class HiddenTilemap : MonoBehaviour
{
    public Tilemap tilemap; // Assign in the inspector
    private Coroutine fadeCoroutine;
    private bool isRevealing = false; // Flag to prevent re-triggering while fading

    private void Start()
    {
        // Error handling: Make sure a Tilemap is assigned
        if (tilemap == null)
        {
            Debug.LogError("Tilemap not assigned in the Inspector for HiddenTilemap script on GameObject: " + gameObject.name);
            this.enabled = false; // Disable script if no tilemap is assigned
            return;
        }

        // Ensure the tilemap starts hidden
        tilemap.color = new Color(1f, 1f, 1f, 0f); // Fully transparent initially
        tilemap.gameObject.SetActive(false);
        isRevealing = false;
    }

    // ADD THIS UPDATE METHOD
    private void Update()
    {
        // Check if the 'Q' key was pressed down this frame AND if it's not already revealing
        if (Input.GetKeyDown(KeyCode.Q) && !isRevealing)
        {
            Reveal();
        }
    }

    public void Reveal()
    {
        if (tilemap == null) return; // Extra safety check

        Debug.Log("Tilemap Revealed!");
        isRevealing = true; // Set the flag

        // Stop any existing fadeout if Reveal is called again (e.g., by another script)
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        // Make the tilemap visible before starting the fade
        tilemap.gameObject.SetActive(true);
        tilemap.color = new Color(1f, 1f, 1f, 1f); // Reset to fully visible

        // Start the fade out process
        fadeCoroutine = StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        float fadeDuration = 3f;
        float elapsedTime = 0f;
        Color color = tilemap.color; // Get the initial (fully visible) color

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            // Calculate alpha based on elapsed time
            // Mathf.Clamp01 ensures the ratio stays between 0 and 1
            color.a = Mathf.Lerp(1f, 0f, Mathf.Clamp01(elapsedTime / fadeDuration));
            tilemap.color = color; // Apply the new color (with updated alpha)
            yield return null; // Wait for the next frame
        }

        // Ensure it's fully faded and deactivated at the end
        color.a = 0f;
        tilemap.color = color;
        tilemap.gameObject.SetActive(false);

        // Reset the state
        isRevealing = false;
        fadeCoroutine = null;
    }
}