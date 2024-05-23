using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine.Assertions.Must;

public class SetNav : MonoBehaviour
{
    public EstimateData estimateData;
    public GameObject targets;
    public Dictionary<string, float> locations = new Dictionary<string, float>();
    private List<Target> navigationTargetObjects = new List<Target>();
    public TextMeshProUGUI locationNameTMP;

    [SerializeField]
    private TextMeshProUGUI ArrivedAtDestinationText;
    private string currentLocation;
    private string currentDest;
    //private string finalDest;
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

    private void SetCurrentNavigationTarget(string selectedText)
    {
        targetPosition = Vector3.zero;

        Target currentTarget = navigationTargetObjects.Find(x => x.Name.Equals(selectedText));
        if (currentTarget != null)
        {
            currentDest += selectedText;
            if (selectedText[0] == currentLocation[0] && selectedText[2] == currentLocation[2])
            {
                targetPosition = currentTarget.PositionObject.transform.position;
                //currentDest = currentLocation + " S " + selectedText + " E, " + selectedText[0] + currentLocation[0] + selectedText[2] + currentLocation[2];
            }
            else if (selectedText[0] == currentLocation[0])
            {
                Transform parent = targets.transform.Find(currentLocation[0].ToString() + currentLocation[1].ToString() + currentLocation[2].ToString());
                targetPosition = parent.Find(estimateData.getClosestElevator()).transform.position;
                currentDest = parent.Find(estimateData.getClosestElevator()).name + " only 0";
            }
        }
    }


    public void CollectTargets(string location)
    {
        navigationTargetObjects.Clear();
        //string a = "C04";
        //string b = "C05";
        // GameObject loc = targets.transform.Find(a.Substring(0, Mathf.Min(3, a.Length))).gameObject;
        // //GameObject loc2 = targets.transform.GetChild(1).gameObject;
        // for (int i = 0; i < loc.transform.childCount; i++)
        // {
        //     Target newTarget = new Target();
        //     newTarget.Name = loc.transform.GetChild(i).name;
        //     newTarget.PositionObject = loc.transform.GetChild(i).gameObject;
        //     navigationTargetObjects.Add(newTarget);
        // }
        // for (int i = 0; i < loc2.transform.childCount; i++)
        // {
        //     Target newTarget = new Target();
        //     newTarget.Name = loc2.transform.GetChild(i).name;
        //     newTarget.PositionObject = loc2.transform.GetChild(i).gameObject;
        //     navigationTargetObjects.Add(newTarget);
        // }

        // GameObject loc2 = targets.transform.Find("C04").gameObject;
        // for (int i = 0; i < loc2.transform.childCount; i++)
        // {
        //     Target newTarget = new Target();
        //     newTarget.Name = loc2.transform.GetChild(i).name;
        //     newTarget.PositionObject = loc2.transform.GetChild(i).gameObject;
        //     navigationTargetObjects.Add(newTarget);
        // }
        foreach (Transform child in targets.transform)
        {
            foreach (Transform grandChild in child)
            {
                Target newTarget = new Target();
                newTarget.Name = grandChild.name;
                newTarget.PositionObject = grandChild.gameObject;
                navigationTargetObjects.Add(newTarget);
            }
        }
    }

    public void SetCurrentLocation(string location)
    {
        currentLocation = location;
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
        locationNameTMP.text = "";
        foreach (Target target in navigationTargetObjects)
        {
            if (target.Name[0] == currentLocation[0] && target.Name[2] == currentLocation[2])
            {
                SetCurrentNavigationTarget(target.Name);
                lineToggle = true;
                NavMesh.CalculatePath(position.position, targetPosition, NavMesh.AllAreas, path);
                line.positionCount = path.corners.Length;
                line.SetPositions(path.corners);
                locations.Add(target.Name, CalculateLineLength(path.corners));
            }
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