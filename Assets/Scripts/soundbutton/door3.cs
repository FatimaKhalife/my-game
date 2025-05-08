using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Door3 : MonoBehaviour
{
    private Animator animator;
    private bool isOpening = false;

    public ButtonSequencePuzzle puzzleManager;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isOpening)
        {
            if (puzzleManager != null && puzzleManager.IsPuzzleSolved())
            {
                Debug.Log("Puzzle completed! Opening door...");
                StartCoroutine(OpenDoor());
            }
        }
    }

    private IEnumerator OpenDoor()
    {
        isOpening = true;
        animator.SetTrigger("Open");
        yield return new WaitForSeconds(1f);

        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(nextSceneIndex);
        else
            SceneManager.LoadScene("Main Menu");
    }
}
