using UnityEngine;

public class SliderPuzzleTrigger : MonoBehaviour
{
    [Header("UI Panel to show")]
    public GameObject sliderPanel;

    private bool isPlayerNearby = false;
    public SliderPuzzleManager puzzleManager;

    private void Start()
    {
        if (sliderPanel != null)
        {
            sliderPanel.SetActive(false);  // Ensure panel is hidden initially
        }
    }

    private void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.P))
        {
            if (sliderPanel != null)
            {
                sliderPanel.SetActive(true);  // Show the panel when P is pressed near the object
                puzzleManager.Freq();
               
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;

            if (sliderPanel != null)
            {
                sliderPanel.SetActive(false); // Optionally hide when player leaves
            }
        }
    }
}
