using UnityEngine;
using UnityEngine.UI; 

public class HideHintOnRightClick : MonoBehaviour
{
    public GameObject hintTextObject; 
    private bool hasRightClicked = false;

    void Update()
    {
        if (!hasRightClicked && Input.GetMouseButtonDown(0)) 
        {
            hasRightClicked = true;
            hintTextObject.SetActive(false);
        }
    }
}
