using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Tilemaps; // For Tilemap support

public enum ColorType { Red, Green, Yellow }

public class SequenceManager : MonoBehaviour
{
    public TextMeshProUGUI consoleText;
    public int sequenceLength = 4;

    public GameObject lockUI;                     // Panel containing 4 TMP_InputFields and a Submit button
    public TMP_InputField[] digitInputs;          // Input fields for entering the 4-digit code

    public Animator chestAnimator;                // Chest animation controller (optional)
    public Tilemap hiddenTilemap;                 // Tilemap to reveal after unlocking

    private List<ColorType> colorSequence = new List<ColorType>();
    private List<string> playedNotes = new List<string>();

    private int currentInputIndex = 0;
    private bool playerCanInput = false;
    private bool hasSequenceStarted = false;
    private bool chestUnlocked = false;

    private void Start()
    {
        consoleText.text = "Approach to start!";
        consoleText.color = Color.white;

        lockUI.SetActive(false);

        if (hiddenTilemap != null)
        {
            hiddenTilemap.gameObject.SetActive(false); // Make tilemap invisible at start
        }
    }

    private void Update()
    {
        if (hasSequenceStarted && !playerCanInput && !chestUnlocked && Input.GetKeyDown(KeyCode.P))
        {
            lockUI.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasSequenceStarted && other.CompareTag("Player"))
        {
            hasSequenceStarted = true;
            GenerateSequence();
        }
    }

    public void GenerateSequence()
    {
        colorSequence.Clear();
        playedNotes.Clear();

        for (int i = 0; i < sequenceLength; i++)
        {
            colorSequence.Add((ColorType)Random.Range(0, 3));
        }

        StartCoroutine(ShowSequence());
    }

    IEnumerator ShowSequence()
    {
        playerCanInput = false;
        currentInputIndex = 0;

        foreach (var color in colorSequence)
        {
            ShowColorOnConsole(color);
            yield return new WaitForSeconds(1f);
        }

        consoleText.text = "Your turn!";
        consoleText.color = Color.white;
        playerCanInput = true;
    }

    void ShowColorOnConsole(ColorType color)
    {
        consoleText.text = color.ToString();
        consoleText.color = GetRandomDisplayColor();
    }

    Color GetRandomDisplayColor()
    {
        Color[] possibleColors = { Color.red, Color.green, Color.yellow };
        return possibleColors[Random.Range(0, possibleColors.Length)];
    }

    public void ReceivePlayerInput(ColorType input)
    {
        if (!playerCanInput) return;

        if (input == colorSequence[currentInputIndex])
        {
            currentInputIndex++;

            if (currentInputIndex >= colorSequence.Count)
            {
                consoleText.text = "Success! Press P to ADD and unlock.";
                consoleText.color = Color.white;
                playerCanInput = false;
            }
        }
        else
        {
            consoleText.text = "Wrong! Restarting...";
            consoleText.color = Color.red;
            playerCanInput = false;
            StartCoroutine(RestartAfterDelay());
        }
    }

    IEnumerator RestartAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        GenerateSequence();
    }

    public void AddNoteToSequence(string noteName)
    {
        if (playedNotes.Count < sequenceLength)
        {
            playedNotes.Add(noteName);
            Debug.Log("Note added: " + noteName);
        }
    }

    public List<string> GetPlayedNotes()
    {
        return playedNotes.Take(4).ToList();
    }

    public void SubmitCode()
    {
        print(playedNotes.Count);
        if (playedNotes.Count < 4)
        {
            consoleText.text = "Not enough notes!";
            return;
        }

        int[] correctCode = playedNotes.Take(4).Select(ConvertNoteToDigit).ToArray();
        int[] enteredCode = digitInputs.Select(input => int.TryParse(input.text, out int val) ? val : -1).ToArray();

        Debug.Log("Correct code: " + string.Join(", ", correctCode));
        Debug.Log("Entered code: " + string.Join(", ", enteredCode));

        if (correctCode.SequenceEqual(enteredCode))
        {
            chestUnlocked = true;
            lockUI.SetActive(false);
            consoleText.text = "Unlocked!";

            if (hiddenTilemap != null)
            {
                hiddenTilemap.gameObject.SetActive(true); // Reveal tilemap
            }

   
        }
        else
        {
            consoleText.text = "Wrong code!";
        }
    }

    IEnumerator ShowPaperCanvas()
    {
        yield return new WaitForSeconds(1f);
        //paperCanvas.SetActive(true);
    }

    int ConvertNoteToDigit(string note)
    {
        char letter = char.ToUpper(note[0]);  // Ensure uppercase
        int octave = int.Parse(note.Substring(1));
        return (letter - 'A' + 1) + octave;
    }
}
