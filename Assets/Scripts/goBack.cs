using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour
{
    public string mainMenuSceneName = "Main Menu"; // change this to match your scene name

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
