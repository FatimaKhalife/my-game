using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;  // To load the scene

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    [SerializeField] private Animator playerAnimator;  // Serialized field for the player's Animator
    [SerializeField] private Rigidbody2D playerRigidbody;  // Serialized field for the player's Rigidbody2D
    public string deathAnimationTrigger = "Die";  // Death animation trigger name
    public float deathDelay = 2f;  // Delay before restarting the level

    private bool isDead = false;

    private void Update()
    {
        fill.color = Color.red;
        // If health reaches 0 and the player is not already dead
        if (slider.value <= 0 && !isDead)
        {
            isDead = true;
            Die();
        }
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(int health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    // This method will be called when health reaches 0
    private void Die()
    {
            Debug.Log("Player Died, triggering animation");

      
            playerRigidbody.linearVelocity = Vector2.zero;  // Set linear velocity to zero
            playerRigidbody.bodyType = RigidbodyType2D.Kinematic;  // Set Rigidbody2D to kinematic to stop movement
            playerAnimator.SetTrigger(deathAnimationTrigger);
        

        // Restart the level after a delay to allow the death animation to play
           StartCoroutine(RestartLevelAfterDelay(deathDelay));
    }

    // Coroutine to restart the level after the death animation
    private IEnumerator RestartLevelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        // After the level restarts, restore the player’s Rigidbody2D
        if (playerRigidbody != null)
        {
            playerRigidbody.bodyType = RigidbodyType2D.Dynamic;  // Restore Rigidbody2D physics to dynamic
        }
    }
}
