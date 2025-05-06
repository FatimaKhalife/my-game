using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void playgame()
    {
        SceneManager.LoadSceneAsync(1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
