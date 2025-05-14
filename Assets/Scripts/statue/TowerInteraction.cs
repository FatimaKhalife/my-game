using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SoundPuzzleController : MonoBehaviour
{
    public GameObject puzzlePanel; // The full canvas or panel
    public Image[] lightBulbs; // Array of UI images for the bulbs
    public Sprite lightOn;
    public Sprite lightOff;

    public string[] correctSequence = { "Wind", "Girl", "Bell" };
    private string[] playerSequence = new string[3];
    private int inputIndex = 0;

    public SpriteRenderer diamondSpriteRenderer; // Assign the diamond SpriteRenderer in inspector
    public GameObject diamondObject; // Diamond object for positioning

    public GameObject towerObject; // The tower object that player needs to be near
    private bool puzzleSolved = false; // Flag to check if the puzzle is solved

    public float floatSpeed = 1f; // Speed at which the diamond floats
    public float floatAmount = 0.5f; // Amount the diamond moves up and down

    private Vector3 initialDiamondPosition;

    public Animator doorAnimator; // Reference to the Animator of the door
    public Transform player; // Reference to the player (must be assigned in the inspector)
    public float openDistance = 3f; // Distance at which the player can open the panel (in units)

    public void OnButtonPress(string soundName)
    {
        if (inputIndex >= 3 || puzzleSolved) return; // Prevent further input if puzzle is solved

        playerSequence[inputIndex] = soundName;
        lightBulbs[inputIndex].sprite = lightOn;
        inputIndex++;

        if (inputIndex == 3)
            CheckSequence();
    }

    void CheckSequence()
    {
        bool isCorrect = true;
        for (int i = 0; i < correctSequence.Length; i++)
        {
            if (playerSequence[i] != correctSequence[i])
            {
                isCorrect = false;
                break;
            }
        }

        if (isCorrect)
        {
            Debug.Log("Puzzle Solved!");
            puzzlePanel.SetActive(false);
            diamondSpriteRenderer.color = Color.red;
            puzzleSolved = true; // Mark the puzzle as solved

            // Start the diamond floating after the puzzle is solved
            StartCoroutine(FloatDiamond());

            // Trigger the door opening animation
            doorAnimator.SetTrigger("OpenDoor"); // Assuming you have an "OpenDoor" trigger in the Animator
        }
        else
        {
            Debug.Log("Wrong Sequence! Try again.");
            ResetPuzzle();
        }
    }

    // Coroutine for floating the diamond up and down
    IEnumerator FloatDiamond()
    {
        initialDiamondPosition = diamondObject.transform.position; // Save the initial position

        while (puzzleSolved) // Keep floating as long as the puzzle is solved
        {
            float newY = Mathf.PingPong(Time.time * floatSpeed, floatAmount); // Create a floating effect
            diamondObject.transform.position = new Vector3(initialDiamondPosition.x, initialDiamondPosition.y + newY, initialDiamondPosition.z);

            yield return null; // Wait for the next frame
        }
    }

    void ResetPuzzle()
    {
        inputIndex = 0;
        for (int i = 0; i < lightBulbs.Length; i++)
        {
            lightBulbs[i].sprite = lightOff;
        }
    }

    public void OpenPanel() // Call this when player presses "P" near the tower
    {
        if (puzzleSolved) return; // Don't open the panel if the puzzle is solved

        ResetPuzzle();
        puzzlePanel.SetActive(true);
    }

    // Detect when the player presses "P" near the tower
    void Update()
    {
        if (Vector3.Distance(player.position, towerObject.transform.position) <= openDistance && !puzzleSolved)
        {
            if (Input.GetKeyDown(KeyCode.P)) // Only open the panel if the player presses "P"
            {
                OpenPanel(); // Open the panel
            }
        }
    }
}
