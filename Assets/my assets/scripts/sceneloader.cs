using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Header("Main Menu")]
    public string mainMenuScene = "MainMenu";

    public void LoadNextScene()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;

        // There is another rollercoaster scene
        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log("Loading next scene: " + nextIndex);
            SceneManager.LoadScene(nextIndex);
        }
        else
        {
            Debug.Log("Final rollercoaster scene completed.");
            ReturnToMainMenu();
        }
    }

    private void ReturnToMainMenu()
    {
        // Destroy the persistent rollercoaster
        SplineFollower follower = FindObjectOfType<SplineFollower>();

        if (follower != null)
        {
            Destroy(follower.gameObject);
        }

        // Load completely fresh Main Menu
        SceneManager.LoadScene(mainMenuScene);
    }
}