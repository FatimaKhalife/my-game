using UnityEngine;
using UnityEngine.UI;

public class SimonTrigger2D : MonoBehaviour
{
    public GameObject simonPanel;
    public SimonGridGame simonGame;
    public Text rewardText;

    private bool playerInRange = false;
    private bool simonCompleted = false;

    void Start()
    {
        simonPanel.SetActive(false);
        rewardText.gameObject.SetActive(false);

        simonGame.OnSimonCompleted += OnSimonSaysComplete;
    }

    void Update()
    {
        if (playerInRange && !simonCompleted && Input.GetKeyDown(KeyCode.P))
        {
            simonPanel.SetActive(true);
            simonGame.StartGame();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !simonCompleted)
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    void OnSimonSaysComplete()
    {
        simonCompleted = true;
        simonPanel.SetActive(false);
        rewardText.text = "The freq is : " + 21.0;
        rewardText.gameObject.SetActive(true);
    }
}
