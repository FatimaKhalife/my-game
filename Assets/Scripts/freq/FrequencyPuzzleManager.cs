using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SliderPuzzleManager : MonoBehaviour
{
    [Header("Sliders for each note")]
    public Slider[] sliders = new Slider[4];

    [Header("Reference from SequenceManager")]
    public SequenceManager sequenceManager;

    [Header("UI & Visuals")]
    public TMPro.TextMeshProUGUI feedbackText;
    public GameObject sliderPanel;
    public Animator doorAnimator; 

    [Header("Scene Transition")]
    public float delayBeforeTransition = 0f;
    public string nextSceneName; // Set this in the Inspector (or use build index)

    private Dictionary<string, Vector2> validNoteRanges = new Dictionary<string, Vector2>();
    private List<string> puzzleNotes = new List<string>();
    private bool puzzleSolved = false;

    private void Update()
    {
        if (!puzzleSolved && sliderPanel)
            CheckSliders();
    }

    public void Freq()
    {
        validNoteRanges["A3".ToUpper()] = new Vector2(288f, 300f);
        validNoteRanges["C4".ToUpper()] = new Vector2(380f, 393f);
        validNoteRanges["E4".ToUpper()] = new Vector2(459f, 476f);
        validNoteRanges["G4".ToUpper()] = new Vector2(540f, 555f);
        validNoteRanges["A4".ToUpper()] = new Vector2(630f, 647f);
        validNoteRanges["G5".ToUpper()] = new Vector2(731f, 749f);
        validNoteRanges["A5".ToUpper()] = new Vector2(834f, 847f);

        if (sequenceManager != null)
            puzzleNotes = sequenceManager.GetPlayedNotes();

        if (puzzleNotes.Count != 4)
            Debug.LogError("Expected 4 notes from previous puzzle, but got: " + puzzleNotes.Count);
    }

    public void CheckSliders()
    {
        if (puzzleNotes.Count != 4)
        {
            feedbackText.text = "Missing note data!";
            return;
        }

        bool allCorrect = true;

        for (int i = 0; i < 4; i++)
        {
            string note = puzzleNotes[i];
            float sliderValue = sliders[i].value;

            if (!validNoteRanges.ContainsKey(note))
            {
                Debug.LogError("Note range missing for: " + note);
                feedbackText.text = "Unknown note: " + note;
                return;
            }

            Vector2 range = validNoteRanges[note];
            Image handleImage = sliders[i].handleRect.GetComponent<Image>();

            if (sliderValue >= range.x && sliderValue <= range.y)
            {
                handleImage.color = Color.green;
            }
            else
            {
                handleImage.color = Color.red;
                allCorrect = false;
            }
        }

        if (allCorrect && !puzzleSolved)
        {
            puzzleSolved = true;
            feedbackText.text = "Puzzle Solved!";
            Debug.Log("Slider puzzle solved!");
            StartCoroutine(SolveSequence());
        }
        else if (!allCorrect)
        {
            feedbackText.text = "Incorrect! Adjust sliders.";
        }
    }

    private IEnumerator SolveSequence()
    {
        yield return new WaitForSeconds(1f); // Short wait before panel disappears
        sliderPanel.SetActive(false);

        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger("Open"); // Trigger door animation
            yield return new WaitForSeconds(0.5f); // Wait for door animation to finish
        }

        // Optionally transition to the next scene
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            yield return new WaitForSeconds(delayBeforeTransition);
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
