using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.ARFoundation;

public class Reposition : MonoBehaviour
{
    public GameObject user;
    public LineRenderer NavigationLineRenderer; // Assume you have a line renderer to draw the path
    public LineRenderer lineRenderer;
    public GameObject textBg;
    public TextMeshProUGUI text;
    public SetNav setNav;


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

        if (distance != 0)
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
        Vector3 newPosition = new Vector3(targetPosition.x, user.transform.position.y, targetPosition.z); // Keep the original Y position
        user.transform.position = newPosition;
    }

    public Vector3 FindNearestNavMeshPosition(Vector3 position, out float distance)
    {
        distance = float.MaxValue; // Initialize with a large value

        NavMeshHit hit;
        Vector3 flatPosition = new Vector3(position.x, 0, position.z); // Ignore Y-axis for the search

        if (NavMesh.SamplePosition(flatPosition, out hit, 5.0f, NavMesh.AllAreas)) //search
        {
            Vector3 targetPosition = new Vector3(hit.position.x, position.y, hit.position.z); // Ignore Y-axis for the movement
            distance = Vector3.Distance(new Vector3(position.x, 0, position.z), new Vector3(hit.position.x, 0, hit.position.z));
            return targetPosition;
        }
        else if (setNav.GetIsNav())
        {
            StartCoroutine(AskForQR());
        }
        return position;
    }

    IEnumerator AskForQR()
    {
        text.text = "Tracking lost, please scan the closest QR code";
        textBg.gameObject.SetActive(true);

        // Wait for 5 seconds
        yield return new WaitForSeconds(5f);

        // Hide the object after 5 seconds
        textBg.gameObject.SetActive(false);
    }
}
