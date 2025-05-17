using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour
{
    public string mainMenuSceneName = "Main Menu";
    public GameObject panelToClose;


    public void GoToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void ClosePanel()
    {
        panelToClose.SetActive(false);
    }
}
