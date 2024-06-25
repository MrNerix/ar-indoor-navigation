using UnityEngine;
using UnityEngine.AI;

public class Reposition : MonoBehaviour
{
    public float speed = 20.0f; // Speed at which the object moves
    public GameObject cameraOffset; // The Camera Offset object
    public GameObject user; // The Main Camera object
    public LineRenderer NavigationLineRenderer; // Assume you have a line renderer to draw the path
    public LineRenderer lineRenderer;
    //private TrackedPoseDriver trackedPoseDriver;

    void Start()
    {
        SetupLineRenderer();
    }

    void Update()
    {
        float distance;
        Vector3 nearestPosition = FindNearestNavMeshPosition(user.transform.position, out distance);
        Debug.Log("Distance to nearest NavMesh position: " + distance);
        DrawPathToNavMesh(nearestPosition);

        if (distance > 0.1f)
        {
            SetPositionToNavMesh(nearestPosition);
        }
    }

    void SetupLineRenderer()
    {
        lineRenderer.startWidth = 0.5f;
        lineRenderer.endWidth = 0.5f;
    }

    void DrawPathToNavMesh(Vector3 targetPosition)
    {
        lineRenderer.SetPosition(0, user.transform.position);
        lineRenderer.SetPosition(1, targetPosition);
    }

    void SetPositionToNavMesh(Vector3 targetPosition)
    {
        //Debug.Log("Setting position to NavMesh. TrackedPoseDriver enabled: " + (trackedPoseDriver != null && trackedPoseDriver.enabled));

        // Move the Camera Offset instead of the Main Camera
        Vector3 newPosition = new Vector3(targetPosition.x, cameraOffset.transform.position.y, targetPosition.z); // Ignore Y-axis for the movement
        cameraOffset.transform.position = newPosition;
        Debug.Log("New position set: " + newPosition);
    }

    public Vector3 FindNearestNavMeshPosition(Vector3 position, out float distance)
    {
        distance = float.MaxValue; // Initialize with a large value

        NavMeshHit hit;
        Vector3 flatPosition = new Vector3(position.x, 0, position.z); // Ignore Y-axis for the search

        if (NavMesh.SamplePosition(flatPosition, out hit, 10.0f, NavMesh.AllAreas)) // Increased search radius to 10.0f
        {
            Vector3 targetPosition = new Vector3(hit.position.x, 0, hit.position.z); // Ignore Y-axis for both distance and movement
            distance = Vector3.Distance(new Vector3(position.x, 0, position.z), targetPosition);
            return targetPosition;
        }

        Debug.LogWarning("Could not find position on NavMesh.");
        return position;
    }

    private float CalculateLineLength(Vector3[] corners)
    {
        float length = 0f;
        for (int i = 0; i < corners.Length - 1; i++)
        {
            length += Vector3.Distance(corners[i], corners[i + 1]);
        }
        return length;
    }
}
