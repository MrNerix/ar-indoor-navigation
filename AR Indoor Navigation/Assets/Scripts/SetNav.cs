using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class SetNav : MonoBehaviour
{

    [SerializeField]
    private TMP_Dropdown navigationTargetDropDown;
    [SerializeField]
    private List<Target> navigationTargetObjects = new List<Target>();

    private NavMeshPath path;
    private LineRenderer line;
    private Vector3 targetPosition = Vector3.zero;
    private bool lineToggle = false;

    public GameObject navMangager;

    // Start is called before the first frame update
    void Start()
    {
        path = new NavMeshPath();
        line = transform.GetComponent<LineRenderer>();
        line.enabled = lineToggle;

        navMangager = GameObject.Find("NavigationManager");
        SetCurrentNavigationTarget(navMangager.GetComponent<TargetValue>().GetTargetedText());

    }

    // Update is called once per frame
    private void Update()
    {

        if (lineToggle && targetPosition != Vector3.zero)
        {
            NavMesh.CalculatePath(transform.position, targetPosition, NavMesh.AllAreas, path);
            line.positionCount = path.corners.Length;
            line.SetPositions(path.corners);
            float length = CalculateLineLength(path.corners);
            //Debug.Log("Line length: " + length);
        }
    }

    public void SetCurrentNavigationTarget(string selectedText)
    {
        targetPosition = Vector3.zero;
        //string selectedText = navigationTargetDropDown.options[selectedValue].text;
        Target currentTarget = navigationTargetObjects.Find(x => x.Name.Equals(selectedText));
        if (currentTarget != null)
        {
            targetPosition = currentTarget.PositionObject.transform.position;
        }
    }

    public void ToggleVisibility()
    {
        lineToggle = !lineToggle;
        line.enabled = lineToggle;
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