using UnityEngine;
using UnityEngine.Splines;

public class SimpleSplineFollower : MonoBehaviour
{
    public SplineContainer splineContainer;
    public float moveSpeed = 2f;
    public float verticalOffset = 1f; // 🆕 Height offset from the spline

    private float distanceTraveled = 0f;
    private float splineLength;

    void Start()
    {
        if (splineContainer != null)
            splineLength = splineContainer.Spline.GetLength();
    }

    void Update()
    {
        if (splineContainer == null || splineLength <= 0f) return;

        distanceTraveled += moveSpeed * Time.deltaTime;
        float t = distanceTraveled / splineLength;
        t = Mathf.Clamp01(t);

        // Evaluate in local space
        Vector3 localPos = splineContainer.Spline.EvaluatePosition(t);
        Vector3 localTan = splineContainer.Spline.EvaluateTangent(t);
        Vector3 localUp = splineContainer.Spline.EvaluateUpVector(t);

        // Apply vertical offset in the spline's up direction
        localPos += localUp.normalized * verticalOffset;

        // Convert to world space
        Vector3 worldPos = splineContainer.transform.TransformPoint(localPos);
        Vector3 worldTan = splineContainer.transform.TransformDirection(localTan);
        Vector3 worldUp = splineContainer.transform.TransformDirection(localUp);

        transform.position = worldPos;
        transform.rotation = Quaternion.LookRotation(worldTan, worldUp);
    }
}
