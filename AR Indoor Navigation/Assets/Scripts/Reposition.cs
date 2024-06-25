using UnityEngine;
using UnityEngine.AI;

public class Reposition : MonoBehaviour
{
    public float speed = 20.0f; // Speed at which the object moves
    public GameObject user;
    //public GameObject Line;
    public LineRenderer NavigationLineRenderer; // Assume you have a line renderer to draw the path
    public LineRenderer lineRenderer;
    void Start()
    {
        //NavigationLineRenderer = GetComponent<LineRenderer>();
        SetupLineRenderer();

    }
    void Update()
    {
        float distance;
        Vector3 nearestPosition = FindNearestNavMeshPosition(transform.position, out distance);
        Debug.Log("Distance to nearest NavMesh position: " + distance);
        DrawPathToNavMesh(nearestPosition);
    }
    void SetupLineRenderer()
    {
        //lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.5f;
        lineRenderer.endWidth = 0.5f;
    }
    void DrawPathToNavMesh(Vector3 targetPosition)
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, targetPosition);
    }

    bool IsLineDrawn()
    {
        return NavigationLineRenderer.positionCount > 1;
    }
    public Vector3 FindNearestNavMeshPosition(Vector3 position, out float distance)
    {
        distance = float.MaxValue; // Initialize with a large value

        NavMeshHit hit;
        Vector3 flatPosition = new Vector3(position.x, 0, position.z); // Ignore Y-axis for the search

        if (NavMesh.SamplePosition(flatPosition, out hit, 15.0f, NavMesh.AllAreas)) // Increased search radius to 5.0f
        {
            Vector3 targetPosition = new Vector3(hit.position.x, transform.position.y, hit.position.z); // Ignore Y-axis for the movement
            distance = Vector3.Distance(new Vector3(position.x, 0, position.z), new Vector3(hit.position.x, 0, hit.position.z));
            if (!IsLineDrawn())
            {
                user.transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            }
            return targetPosition;
        }

        Debug.LogWarning("Could not find position on NavMesh.");
        return position;
    }
}