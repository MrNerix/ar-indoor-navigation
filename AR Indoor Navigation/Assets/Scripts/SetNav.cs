using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class SetNav : MonoBehaviour
{
    private Dictionary<string, float> locations = new Dictionary<string, float>();

    [SerializeField]
    private List<Target> navigationTargetObjects = new List<Target>();
    public TextMeshProUGUI locationNameTMP;

    [SerializeField]
    private TextMeshProUGUI ArrivedAtDestinationText;
    private NavMeshPath path;
    private LineRenderer line;
    private Vector3 targetPosition = Vector3.zero;
    private bool lineToggle = false;
    private bool isFinished = false;

    public GameObject navMangager;

    // Start is called before the first frame update
    void Start()
    {
        path = new NavMeshPath();
        line = transform.GetComponent<LineRenderer>();
        line.enabled = lineToggle;
        //Debug.Log("Startuota");
        if (GameObject.Find("NavigationManager") != null)
        {
            navMangager = GameObject.Find("NavigationManager");
            SetCurrentNavigationTarget(navMangager.GetComponent<SceneLoader>().GetTargetedText());
            locationNameTMP.text = navMangager.GetComponent<SceneLoader>().GetTargetedText();
        }
        else
        {
            //Debug.Log("Checkinu...");

            foreach (Target target in navigationTargetObjects)
            {
                SetCurrentNavigationTarget(target.Name);
                lineToggle = true;
                Debug.Log("nusetintas targetas i " + target.Name + ". Ar linija ijungta? " + lineToggle);
                NavMesh.CalculatePath(transform.position, targetPosition, NavMesh.AllAreas, path);
                line.positionCount = path.corners.Length;
                line.SetPositions(path.corners);
                //Debug.Log("bandau skaiciuot ir gaunu " + CalculateLineLength(path.corners));
                locations.Add(target.Name, CalculateLineLength(path.corners));
                Debug.Log("Path length to " + target.Name + ": " + CalculateLineLength(path.corners));
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        Debug.Log("Line length: " + CalculateLineLength(path.corners) + ". target is " + navMangager.GetComponent<SceneLoader>().GetTargetedText());

        if (lineToggle && targetPosition != Vector3.zero)
        {

            NavMesh.CalculatePath(transform.position, targetPosition, NavMesh.AllAreas, path);
            line.positionCount = path.corners.Length;
            line.SetPositions(path.corners);
            if (isFinished == false && CalculateLineLength(path.corners) != 0 && CalculateLineLength(path.corners) <= 2)
            {
                ArrivedAtDestinationText.text = "You have arrived at your destination: " + navMangager.GetComponent<SceneLoader>().GetTargetedText();
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
        Debug.Log("set true");
        // Show the object
        ArrivedAtDestinationText.gameObject.SetActive(true);

        // Wait for 5 seconds
        yield return new WaitForSeconds(5f);

        // Hide the object after 5 seconds
        ArrivedAtDestinationText.gameObject.SetActive(false);
        Debug.Log("set false");
    }
}