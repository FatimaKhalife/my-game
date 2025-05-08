using UnityEngine;
using System.Collections;

public class ButtonSequencePuzzle : MonoBehaviour
{
    public int[] requiredClicks = { 4, 1, 1, 1 }; // Example sequence
    private int[] currentClicks;

    public AudioSource[] buttonSounds;  // Assign 4 AudioSources for the button sounds
    public AudioSource failSound;       // Assign a fail sound AudioSource in Inspector
    public AudioSource rightAnswerSound; // Assign a right answer sound AudioSource in Inspector

    private bool puzzleSolved = false;

    void Start()
    {
        currentClicks = new int[requiredClicks.Length];
    }

    public void RegisterClick(int buttonIndex)
    {
        if (puzzleSolved || buttonIndex < 0 || buttonIndex >= requiredClicks.Length)
            return;

        currentClicks[buttonIndex]++;
        buttonSounds[buttonIndex].Play();

        if (currentClicks[buttonIndex] > requiredClicks[buttonIndex])
        {
            Debug.Log("Incorrect sequence. Resetting...");
            StartCoroutine(PlayFailSoundWithDelay()); // Start the coroutine with delay
            ResetPuzzle();
            return;
        }

        CheckIfSolved();
    }

    private void CheckIfSolved()
    {
        for (int i = 0; i < requiredClicks.Length; i++)
        {
            if (currentClicks[i] != requiredClicks[i])
                return;
        }

        Debug.Log("Puzzle Solved!");
        puzzleSolved = true;
        StartCoroutine(PlayRightAnswerSoundWithDelay()); // Play the correct sound when puzzle is solved
    }

    private void ResetPuzzle()
    {
        for (int i = 0; i < currentClicks.Length; i++)
            currentClicks[i] = 0;
    }

    private IEnumerator PlayFailSoundWithDelay()
    {
        yield return new WaitForSeconds(0.5f); // Delay before playing the fail sound
        failSound.Play();  // Play the fail sound
    }

    private IEnumerator PlayRightAnswerSoundWithDelay()
    {
        yield return new WaitForSeconds(0.5f); // Optional delay before playing the right answer sound
        rightAnswerSound.Play();  // Play the correct answer sound
    }

    public bool IsPuzzleSolved()
    {
        return puzzleSolved;
    }
}
