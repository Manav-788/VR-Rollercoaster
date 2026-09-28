using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplineFollower : MonoBehaviour
{
    public SplineContainer splineContainer;
    public float speed = 5f;
    public float verticalOffset = 0f;

    [HideInInspector] public float t = 0f; 
    private float splineLength;
    private Spline spline;
    private bool hasReachedEnd = false;

    void Start()
    {
        DontDestroyOnLoad(gameObject); // persists between scenes

        if (splineContainer == null)
        {
            Debug.LogError("SplineContainer not assigned!");
            enabled = false;
            return;
        }

        SetupSpline(splineContainer);
        ResetProgress();
    }

    void Update()
    {
        if (splineLength <= 0f) return; // safety check

        if (t < 1f)
        {
            float delta = (speed * Time.deltaTime) / splineLength;
            t = Mathf.Clamp01(t + delta);

            Vector3 localPos = SplineUtility.EvaluatePosition(spline, t);
            Vector3 localTangent = ((Vector3)SplineUtility.EvaluateTangent(spline, t)).normalized;

            Vector3 worldPos = splineContainer.transform.TransformPoint(localPos);
            Vector3 worldTangent = splineContainer.transform.TransformDirection(localTangent);

            Vector3 worldUp = splineContainer.transform.up;
            worldPos += worldUp * verticalOffset;

            Quaternion rotation = Quaternion.LookRotation(worldTangent);
            transform.SetPositionAndRotation(worldPos, rotation);
        }
        else if (!hasReachedEnd)
        {
            hasReachedEnd = true;
            Debug.Log("Reached end of spline — loading next scene.");

            SceneLoader loader = FindObjectOfType<SceneLoader>();
            if (loader != null)
                StartCoroutine(LoadSceneWithDelay(loader));
        }
    }

    private IEnumerator LoadSceneWithDelay(SceneLoader loader)
    {
        yield return null; // wait one frame to prevent double load
        loader.LoadNextScene();
    }

    public void AssignNewSpline(SplineContainer newSplineContainer)
    {
        SetupSpline(newSplineContainer);
        ResetProgress();
    }

    private void SetupSpline(SplineContainer container)
    {
        splineContainer = container;
        spline = container.Spline;

        float4x4 localToWorld = float4x4.TRS(
            container.transform.position,
            container.transform.rotation,
            container.transform.lossyScale
        );

        splineLength = SplineUtility.CalculateLength(spline, localToWorld);
        Debug.Log("Spline length set to: " + splineLength);
    }

    public void ResetProgress()
    {
        t = 0f;
        hasReachedEnd = false;

        if (splineContainer != null)
        {
            Vector3 startPos = SplineUtility.EvaluatePosition(spline, 0f);
            Vector3 worldPos = splineContainer.transform.TransformPoint(startPos);
            transform.position = worldPos;
            transform.rotation = Quaternion.identity;
        }
    }
}
