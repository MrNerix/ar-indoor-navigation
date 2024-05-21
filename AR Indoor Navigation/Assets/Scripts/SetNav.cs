using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;

public class SetNav : MonoBehaviour
{
    public EstimateData estimateData;
    public GameObject targets;
    public Dictionary<string, float> locations = new Dictionary<string, float>();
    private List<Target> navigationTargetObjects = new List<Target>();
    public TextMeshProUGUI locationNameTMP;

    [SerializeField]
    private TextMeshProUGUI ArrivedAtDestinationText;
    private NavMeshPath path;
    private LineRenderer line;
    private Vector3 targetPosition = Vector3.zero;
    private bool lineToggle = false;
    private bool isFinished = false;

    private GameObject navManager;

    // Start is called before the first frame update
    void Start()
    {
        path = new NavMeshPath();
        line = transform.GetComponent<LineRenderer>();
        line.enabled = lineToggle;
    }

    // Update is called once per frame
    private void Update()
    {
        if (lineToggle && targetPosition != Vector3.zero)
        {
            NavMesh.CalculatePath(transform.position, targetPosition, NavMesh.AllAreas, path);
            line.positionCount = path.corners.Length;
            line.SetPositions(path.corners);
            if (isFinished == false && CalculateLineLength(path.corners) != 0 && CalculateLineLength(path.corners) <= 2)
            {
                ArrivedAtDestinationText.text = "You have arrived at your destination: " + navManager.GetComponent<SceneLoader>().GetTargetedText();
                isFinished = true;
                StartCoroutine(ShowAndHideObject());
            }
        }
    }

    public void SetCurrentNavigationTarget(string selectedText)
    {
        targetPosition = Vector3.zero;
        Target currentTarget = navigationTargetObjects.Find(x => x.Name.Equals(selectedText));
        if (currentTarget != null)
        {
            targetPosition = currentTarget.PositionObject.transform.position;
        }
    }

    public void CollectTargets(string location)
    {
        navigationTargetObjects.Clear();
        GameObject loc = targets.transform.Find(location.Substring(0, Mathf.Min(3, location.Length))).gameObject;
        for (int i = 0; i < loc.transform.childCount; i++)
        {
            Target newTarget = new Target();
            newTarget.Name = loc.transform.GetChild(i).name;
            newTarget.PositionObject = loc.transform.GetChild(i).gameObject;
            navigationTargetObjects.Add(newTarget);
        }
        // if (GameObject.Find("NavigationManager") != null)
        // {
        //     navManager = GameObject.Find("NavigationManager");
        //     SetCurrentNavigationTarget(navManager.GetComponent<SceneLoader>().GetTargetedText());
        //     locationNameTMP.text = navManager.GetComponent<SceneLoader>().GetTargetedText();
        // }
    }

    public void ToggleVisibility()
    {
        lineToggle = !lineToggle;
        line.enabled = lineToggle;
    }

    public void VoidTargetPosition()
    {
        targetPosition = Vector3.zero;
        line.enabled = false;
        lineToggle = false;
    }

    public void CalculateAllDistances(Transform position)
    {
        locations.Clear();
        foreach (Target target in navigationTargetObjects)
        {
            SetCurrentNavigationTarget(target.Name);
            lineToggle = true;
            NavMesh.CalculatePath(position.position, targetPosition, NavMesh.AllAreas, path);
            line.positionCount = path.corners.Length;
            line.SetPositions(path.corners);
            locations.Add(target.Name, CalculateLineLength(path.corners));
        }
        estimateData.CollectNewEstimates(locations);
        if (GameObject.Find("NavigationManager") != null)
        {
            navManager = GameObject.Find("NavigationManager");
            SetCurrentNavigationTarget(navManager.GetComponent<SceneLoader>().GetTargetedText());
            locationNameTMP.text = navManager.GetComponent<SceneLoader>().GetTargetedText();
        }
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

    IEnumerator ShowAndHideObject()
    {
        ArrivedAtDestinationText.gameObject.SetActive(true);

        // Wait for 5 seconds
        yield return new WaitForSeconds(5f);

        // Hide the object after 5 seconds
        ArrivedAtDestinationText.gameObject.SetActive(false);
    }
}