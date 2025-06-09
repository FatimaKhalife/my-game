using UnityEngine;
using UnityEngine.UI;

public class HideHintOnFirstF : MonoBehaviour
{
    public GameObject hintTextObject; 
    private bool hasPressedF = false;

    void Update()
    {
        if (!hasPressedF && Input.GetKeyDown(KeyCode.F))
        {
            hasPressedF = true;
            hintTextObject.SetActive(false);
        }
        if (!hasPressedF && Input.GetKeyDown(KeyCode.Q))
        {
            hasPressedF = true;
            hintTextObject.SetActive(false);
        }
        if (!hasPressedF && Input.GetKeyDown(KeyCode.E))
        {
            hasPressedF = true;
            hintTextObject.SetActive(false);
        }
    }
}
