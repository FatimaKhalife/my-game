using System.Collections.Generic;
using UnityEngine;

public class PianoPuzzle : MonoBehaviour
{
    public List<int> correctSequence = new List<int> { 1, 2, 3, 1 }; 
    private List<int> playerSequence = new List<int>();
    public GameObject panelToClose; 

    public void PianoKeyPressed(int keyNumber)
    {
        playerSequence.Add(keyNumber);

        // Show current sequence in Console
        Debug.Log("Current Sequence: " + string.Join(" ", playerSequence));

        // Check for mismatch
        for (int i = 0; i < playerSequence.Count; i++)
        {
            if (playerSequence[i] != correctSequence[i])
            {
                Debug.Log("Wrong sequence! Resetting.");
                playerSequence.Clear();
                return;
            }
        }

        // Sequence completed
        if (playerSequence.Count == correctSequence.Count)
        {
            Debug.Log("Completed");
            panelToClose.SetActive(false);
            playerSequence.Clear();
        }
    }
}
