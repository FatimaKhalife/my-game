using UnityEngine;

public class ColorButton : MonoBehaviour
{
    public ColorType buttonColor;
    public SequenceManager sequenceManager; // Drag and drop in Inspector
    public SoundCluePlayer SoundClue;




    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            sequenceManager.ReceivePlayerInput(buttonColor);
            SoundClue.PlayVoiceClue();
        }
    }
}
