using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SoundPuzzleController : MonoBehaviour
{
    [Header("Puzzle UI")]
    public GameObject puzzlePanel; // The puzzle UI panel
    public Image[] lightBulbs; // UI images for the bulbs
    public Sprite lightOn;
    public Sprite lightOff;

    [Header("Puzzle Logic")]
    [SerializeField]
    private string[] correctSequence; // Set per tower in Inspector
    private string[] playerSequence;
    private int inputIndex = 0;

    [Header("Puzzle Result")]
    public SpriteRenderer diamondSpriteRenderer; // Assign this in the inspector
    public GameObject diamondObject; // The diamond GameObject (for floating)

    [Header("Proximity Settings")]
    public GameObject towerObject; // The tower GameObject (used for proximity check)
    private bool puzzleSolved = false;

    [Header("Diamond Float Effect")]
    public float floatSpeed = 1f;
    public float floatAmount = 0.5f;
    private Vector3 initialDiamondPosition;

    [Header("Door Interaction")]
    public Animator doorAnimator; // Animator for the door object
    public Transform player; // Reference to the player (assign in inspector)
    public float openDistance = 3f; // Distance to open the panel

    [Header("UI Prompt")]
    public TMP_Text pressPText; // TextMeshPro UI element for "Press P"

    void Start()
    {
        playerSequence = new string[correctSequence.Length];
    }

    void Update()
    {
        float distance = Vector3.Distance(player.position, towerObject.transform.position);

        if (distance <= openDistance && !puzzleSolved)
        {
            pressPText.gameObject.SetActive(true);

            if (Input.GetKeyDown(KeyCode.P))
            {
                OpenPanel();
            }
        }
        else
        {
            pressPText.gameObject.SetActive(false);
        }
    }

    public void OnButtonPress(string soundName)
    {
        if (inputIndex >= correctSequence.Length || puzzleSolved) return;

        playerSequence[inputIndex] = soundName;
        if (inputIndex < lightBulbs.Length)
        {
            lightBulbs[inputIndex].sprite = lightOn;
        }
        inputIndex++;

        if (inputIndex == correctSequence.Length)
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
            puzzleSolved = true;

            StartCoroutine(FloatDiamond());
            doorAnimator.SetTrigger("OpenDoor");
        }
        else
        {
            Debug.Log("Wrong Sequence! Try again.");
            StartCoroutine(ResetWithDelay());
        }
    }

    IEnumerator ResetWithDelay()
    {
        yield return new WaitForSeconds(0.5f); // Optional delay before reset
        ResetPuzzle();
    }

    IEnumerator FloatDiamond()
    {
        initialDiamondPosition = diamondObject.transform.position;

        while (puzzleSolved)
        {
            float newY = Mathf.PingPong(Time.time * floatSpeed, floatAmount);
            diamondObject.transform.position = new Vector3(initialDiamondPosition.x, initialDiamondPosition.y + newY, initialDiamondPosition.z);

            yield return null;
        }
    }

    void ResetPuzzle()
    {
        inputIndex = 0;
        playerSequence = new string[correctSequence.Length];
        for (int i = 0; i < lightBulbs.Length; i++)
        {
            lightBulbs[i].sprite = lightOff;
        }
    }

    public void OpenPanel()
    {
        if (puzzleSolved) return;

        ResetPuzzle();
        puzzlePanel.SetActive(true);
    }
}
