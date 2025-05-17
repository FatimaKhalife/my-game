using UnityEngine;

public class ConsoleInteraction : MonoBehaviour
{
    public GameObject panelToOpen;
    private bool playerIsNear = false;

    void Update()
    {
        if (playerIsNear && Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("P pressed while near console.");
            panelToOpen.SetActive(true);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Entered trigger with: " + other.name);

        if (other.CompareTag("Player"))
        {
            playerIsNear = true;
            Debug.Log("Player is near console.");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            playerIsNear = false;
            panelToOpen.SetActive(false); 
        }
    }
}
