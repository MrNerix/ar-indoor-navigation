using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SetNav : MonoBehaviour
{
    [SerializeField]
    private Camera topDownCamera;
    [SerializeField]
    private GameObject navTargetObject;

    private NavMeshPath path;
    private LineRenderer line;
    private bool lineToggle = false;

    // Start is called before the first frame update
    void Start()
    {
        path = new NavMeshPath();
        line = transform.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
        {
            lineToggle = !lineToggle;
        }
        if (lineToggle)
        {
            NavMesh.CalculatePath(transform.position, navTargetObject.transform.position, NavMesh.AllAreas, path);
            line.positionCount = path.corners.Length;
            line.SetPositions(path.corners);
            line.enabled = true;

            // Calculate and output the length of the line
            //float length = CalculateLineLength(path.corners);
            //Debug.Log("Line length: " + length);
        }
        else
        {
            line.enabled = false;
        }
    }

    // Method to calculate the length of the line based on its corners
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