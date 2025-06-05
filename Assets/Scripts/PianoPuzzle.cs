using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PianoPuzzle : MonoBehaviour
{
    public List<int> correctSequence = new List<int> { 1, 2, 3, 1 };
    private List<int> playerSequence = new List<int>();

    public GameObject panelToClose;
    public TextMeshProUGUI guidanceText;

    public DoorController door; 

    public void PianoKeyPressed(int keyNumber)
    {
        playerSequence.Add(keyNumber);

        Debug.Log("Current Sequence: " + string.Join(" ", playerSequence));

        // Check for mismatch
        for (int i = 0; i < playerSequence.Count; i++)
        {
            if (playerSequence[i] != correctSequence[i])
            {
                Debug.Log("Wrong sequence! Resetting.");
                playerSequence.Clear();
                if (guidanceText != null)
                    guidanceText.text = "Wrong! Try again.";
                return;
            }
        }

        // Partial match
        if (playerSequence.Count < correctSequence.Count)
        {
            if (guidanceText != null)
                guidanceText.text = $"Good! {correctSequence.Count - playerSequence.Count} step(s) left...";
        }

        // Correct sequence
        if (playerSequence.Count == correctSequence.Count)
        {
            Debug.Log("Puzzle completed.");

            if (guidanceText != null)
                guidanceText.text = "Well done! Puzzle completed.";

            if (panelToClose != null)
                panelToClose.SetActive(false);

            if (door != null)
                door.OpenDoor();

            playerSequence.Clear();
        }
    }
}
