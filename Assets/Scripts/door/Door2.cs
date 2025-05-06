using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Door2 : MonoBehaviour
{
    public float targetFrequency = 440f; // The correct frequency to open the door
    public float tolerance = 5f; // Allowed range for the frequency match
    private Animator animator;
    private bool isOpening = false; // Prevent multiple triggers

    public FrequencyAnalyzer frequencyAnalyzer;
    void Start()
    {
        animator = GetComponent<Animator>();
    }




    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isOpening)
        {
            // Get the player's frequency analyzer
            frequencyAnalyzer = collision.GetComponent<FrequencyAnalyzer>();

            if (frequencyAnalyzer != null)
            {
                float playerFrequency = frequencyAnalyzer.GetDominantFrequency();

                if (Mathf.Abs(playerFrequency - targetFrequency) <= tolerance)
                {
                    Debug.Log("Correct frequency detected! Opening door...");
                    StartCoroutine(OpenDoor());
                }
                else
                {
                    Debug.Log("Still adjusting frequency...");
                }
            }
            else
            {
                Debug.Log("No FrequencyAnalyzer found on Player!");
            }
        }
    }


    private IEnumerator OpenDoor()
    {
        isOpening = true; // Prevent multiple triggers
        animator.SetTrigger("Open"); // Trigger door opening animation
        Debug.Log("Triggering Door Animation");

        yield return new WaitForSeconds(1f); // Wait for the animation to finish

        // Load the next level
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("No more levels! Returning to Main Menu."); // Debug
            SceneManager.LoadScene("Main Menu"); // Load the main menu if no more levels
        }
    }
}
