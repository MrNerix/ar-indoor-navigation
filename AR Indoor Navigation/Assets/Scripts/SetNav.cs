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
    public GameObject ArrivedAtDestinationContainer;

    private string currentLocation;
    private string currentDest;
    private NavMeshPath path;
    private LineRenderer line;
    private Vector3 targetPosition = Vector3.zero;
    private bool lineToggle = true;
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
            if (isFinished == false && CalculateLineLength(path.corners) != 0 && CalculateLineLength(path.corners) <= 2 && currentDest == navManager.GetComponent<SceneLoader>().GetTargetedText())
            {
                ArrivedAtDestinationText.text = "You have arrived at your destination: " + currentDest;
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
            currentDest = selectedText;
            if (selectedText[0] == currentLocation[0] || (currentLocation[2] - '0') <= 3)
            {
                if (selectedText[2] == currentLocation[2])
                {
                    targetPosition = currentTarget.PositionObject.transform.position;
                }
                else
                {
                    if ((currentLocation[2] - '0') <= 3)
                    {
                        Transform parent = targets.transform.Find("X" + currentLocation[1].ToString() + currentLocation[2].ToString());
                        targetPosition = parent.Find(estimateData.getClosestElevator(selectedText[0])).transform.position;
                        currentDest = parent.Find(estimateData.getClosestElevator(selectedText[0])).transform.name;
                    }
                    else
                    {
                        Transform parent = targets.transform.Find(currentLocation[0].ToString() + currentLocation[1].ToString() + currentLocation[2].ToString());
                        targetPosition = parent.Find(estimateData.getClosestElevator(selectedText[0])).transform.position;
                        currentDest = parent.Find(estimateData.getClosestElevator(selectedText[0])).transform.name;
                    }
                }
            }
        }
    }


    public void CollectTargets()
    {
        navigationTargetObjects.Clear();
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
            if ((target.Name[0] == currentLocation[0] || (currentLocation[2] - '0') <= 3) && target.Name[2] == currentLocation[2])
            {
                SetCurrentNavigationTarget(target.Name);
                //lineToggle = true;
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
        ArrivedAtDestinationContainer.gameObject.SetActive(true);

        // Wait for 5 seconds
        yield return new WaitForSeconds(5f);

        // Hide the object after 5 seconds
        ArrivedAtDestinationContainer.gameObject.SetActive(false);
    }
}