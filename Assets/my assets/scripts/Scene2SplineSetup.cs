using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Splines;

public class GeneralSplineSetup : MonoBehaviour
{
    public SplineContainer splineContainer; // assign this scene's spline in Inspector

    private void Start()
    {
        // Only run setup for scenes AFTER the first one
        if (SceneManager.GetActiveScene().buildIndex == 0) return;

        SplineFollower follower = FindObjectOfType<SplineFollower>();
        if (follower != null && splineContainer != null)
        {
            Debug.Log("Assigning spline for scene: " + SceneManager.GetActiveScene().name);
            follower.AssignNewSpline(splineContainer);
        }
        else
        {
            Debug.LogWarning("SplineFollower or SplineContainer not found in scene.");
        }
    }
}
