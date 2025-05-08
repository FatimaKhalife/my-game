using UnityEngine;

public class SoundButton : MonoBehaviour
{
    public int buttonIndex;
    public ButtonSequencePuzzle puzzleManager;

    public void OnButtonClick()
    {
        print("ha");
        puzzleManager.RegisterClick(buttonIndex);
    }
}
