using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadNextScene()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;

        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log("Loading scene index: " + nextIndex);
            SceneManager.LoadScene(nextIndex);
        }
        else
        {
            Debug.Log("Reached final scene — no more scenes to load.");
        }
    }
}
