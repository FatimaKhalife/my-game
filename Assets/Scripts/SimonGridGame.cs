using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimonGridGame : MonoBehaviour
{
    [Header("UI References")]
    public Button[] gridButtons;       // Assign 9 buttons
    public Button startButton;         // Start button
    public Text statusText;            // Status message

    public System.Action OnSimonCompleted;

    private List<int> sequence = new List<int>();
    private List<int> playerInput = new List<int>();
    private bool playerTurn = false;
    private int currentRound = 0;
    private int maxRounds = 6;

    void Start()
    {
        startButton.onClick.AddListener(StartGame);

        for (int i = 0; i < gridButtons.Length; i++)
        {
            int index = i;
            gridButtons[i].onClick.AddListener(() => OnGridButtonClicked(index));
        }
    }

    public void StartGame()
    {
        sequence.Clear();
        playerInput.Clear();
        currentRound = 0;
        statusText.text = "Watch the sequence!";
        StartCoroutine(NextRound());
    }

    IEnumerator NextRound()
    {
        if (currentRound >= maxRounds)
        {
            statusText.text = "You Win!";
            playerTurn = false;
            OnSimonCompleted?.Invoke();
            yield break;
        }

        playerTurn = false;
        playerInput.Clear();
        sequence.Add(Random.Range(0, 9));
        currentRound++;

        yield return PlaySequence();

        playerTurn = true;
        statusText.text = "Your Turn!";
    }

    IEnumerator PlaySequence()
    {
        yield return new WaitForSeconds(1f);

        foreach (int index in sequence)
        {
            yield return FlashButton(index);
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator FlashButton(int index)
    {
        Button btn = gridButtons[index];
        Color originalColor = btn.image.color;

        btn.image.color = Color.white;
        yield return new WaitForSeconds(0.4f);
        btn.image.color = originalColor;
    }

    void OnGridButtonClicked(int index)
    {
        if (!playerTurn) return;

        playerInput.Add(index);
        int i = playerInput.Count - 1;

        if (playerInput[i] != sequence[i])
        {
            statusText.text = "❌ Wrong! Game Over!";
            playerTurn = false;
            return;
        }

        if (playerInput.Count == sequence.Count)
        {
            statusText.text = "✅ Correct!";
            playerTurn = false;
            StartCoroutine(WaitAndStartNextRound());
        }
    }

    IEnumerator WaitAndStartNextRound()
    {
        yield return new WaitForSeconds(1.5f);
        statusText.text = "Watch the sequence!";
        yield return NextRound();
    }
}
