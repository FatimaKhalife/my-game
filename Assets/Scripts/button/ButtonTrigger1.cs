using UnityEngine;

public class ButtonTrigger1 : MonoBehaviour
{
    public Animator animator;

  

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("isPressed", true);
            print("pressed");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("isPressed", false);
        }
    }
}
