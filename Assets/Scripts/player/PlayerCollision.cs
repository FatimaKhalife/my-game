using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
public class PlayerCollision : MonoBehaviour
{
    private Animator animator;
    private bool isDead = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isDead && other.CompareTag("Deadly"))
        {
            isDead = true;
            animator.SetTrigger("Die");
            StartCoroutine(RestartAfterAnimation());
        }
    }

    IEnumerator RestartAfterAnimation()
    {
        // Wait for the length of the death animation
        yield return new WaitForSeconds(1.5f); // Replace with actual animation length
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
