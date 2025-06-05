using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour
{
    public Animator doorAnimator;
    public string openTrigger = "Open";

    [Header("Scene Transition")]
    public string sceneToLoad;
  

    private bool isOpen = false;
    private bool hasLoadedScene = false;


    public void OpenDoor()
    {
        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger(openTrigger);
        }

        isOpen = true; 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isOpen && !hasLoadedScene && other.CompareTag("Player"))
        {
            hasLoadedScene = true;
            Invoke(nameof(LoadNextScene), 0);
        }
    }

    private void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("No scene assigned to load.");
        }
    }
}
