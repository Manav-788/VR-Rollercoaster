using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Scene")]
    public string gameSceneName = "RollerCoasterScene";

    [Header("Social Links")]
    public string linkedInURL = "https://www.linkedin.com/";
    public string githubURL = "https://github.com/";

    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void OpenLinkedIn()
    {
        Application.OpenURL(linkedInURL);
    }

    public void OpenGitHub()
    {
        Application.OpenURL(githubURL);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}