using UnityEngine;
using System.Collections.Generic;

public class ButtonTrigger : MonoBehaviour
{
    public GameObject button; // Reference to the physical button object
    public GameObject targetObject; // The object that reacts to the button
    private Animator buttonAnimator;
    private Animator targetObjectAnimator;

    // Use a HashSet to keep track of objects currently on the button
    private HashSet<Collider2D> collidersOnButton = new HashSet<Collider2D>();

    private void Start()
    {
        if (button != null)
        {
            buttonAnimator = button.GetComponent<Animator>(); // Get the button's animator
        }

        if (targetObject != null)
        {
            targetObjectAnimator = targetObject.GetComponent<Animator>(); // Get the target object's animator
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Add the collider to the set
        collidersOnButton.Add(other);

        // Check if the player or box is on the button
        UpdateButtonState();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Remove the collider from the set
        collidersOnButton.Remove(other);

        // Check if the player or box is still on the button
        UpdateButtonState();
    }

    private void UpdateButtonState()
    {
        bool isPressed = false;

        // Check if any of the colliders in the set are the player or the box
        foreach (var collider in collidersOnButton)
        {
            if (collider.CompareTag("Player") || collider.CompareTag("pushable")) // Ensure you have the correct tags
            {
                isPressed = true;
                break;
            }
        }

        // Update the button and target object animations based on the button state
        if (buttonAnimator != null)
        {
            buttonAnimator.SetBool("isPressed", isPressed); // Play button animation
        }

        if (targetObjectAnimator != null)
        {
            targetObjectAnimator.SetBool("isPressed", isPressed); // Play target object animation
        }
    }
}