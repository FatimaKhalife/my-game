using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Door : MonoBehaviour
{
    private bool hasKey = false;
    private Animator animator;
    private bool isOpening = false; // Prevent multiple triggers

    private void Start()
    {
        animator = GetComponent<Animator>(); // Get the Animator component
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name); // Debug

        if (collision.CompareTag("Player") && !isOpening)
        {
            Debug.Log("Player entered the door trigger zone"); // Debug

            // Check if the player has a key
            Key key = collision.GetComponentInChildren<Key>();

            if (key != null)
            {
                Debug.Log("Key detected! Opening door..."); // Debug
                hasKey = true;
                Destroy(key.gameObject); // Remove the key from the player
                StartCoroutine(OpenDoor());
            }
            else
            {
                Debug.Log("No key found! Can't open door."); // Debug
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