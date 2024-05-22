using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Filter : MonoBehaviour
{

    public TMP_Dropdown blockDropdown;
    public TMP_Dropdown floorDropdown;
    public TMP_Dropdown typeDropdown;
    public TMP_Dropdown destinations;
    private bool isDropdownOpen = false;
    private GameObject est;
    private EstimateData estimates;
    // Lists (used before we implement a database)

    public Dictionary<string, float> estimateData = new Dictionary<string, float>();
    public Dictionary<string, List<string>> listDictionary = new Dictionary<string, List<string>>();
    public TextMeshProUGUI[] extraLabels;
    private List<string> filteredOptions = new List<string>();
    private List<string> classrooms = new List<string>();
    private List<string> groupRooms = new List<string>();
    private List<string> wc = new List<string>();
    private List<string> wcHandicapped = new List<string>();
    private List<string> stairs = new List<string>();
    private List<string> elevators = new List<string>();
    private List<string> coffeeSpots = new List<string>();
    private List<string> printers = new List<string>();
    private List<string> lockers = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        est = GameObject.Find("DestinationEstimateData");
        estimates = est.GetComponent<EstimateData>();
        estimateData = estimates.getCurrentEstimates();

        listDictionary.Add("Classroom", classrooms);
        listDictionary.Add("Group Room", groupRooms);
        listDictionary.Add("WC", wc);
        listDictionary.Add("WC (Handicap)", wcHandicapped);
        listDictionary.Add("Stairs", stairs);
        listDictionary.Add("Elevator", elevators);
        listDictionary.Add("Coffee Spot", coffeeSpots);
        listDictionary.Add("Printer", printers);
        listDictionary.Add("Lockers", lockers);

        classrooms.Add("C04.12");
        classrooms.Add("C04.13a");
        classrooms.Add("C04.13b");
        classrooms.Add("C04.16");
        classrooms.Add("C04.18");

        groupRooms.Add("C04.05");
        groupRooms.Add("C04.07");
        groupRooms.Add("C04.08");
        groupRooms.Add("C04.09");
        groupRooms.Add("C04.10");
        groupRooms.Add("C04.11");

        wc.Add("C04.WC_1");
        wc.Add("C04.WC_2");

        wcHandicapped.Add("C04.WC_HC");

        stairs.Add("C04.Stairs_1");
        stairs.Add("C04.Stairs_2");
        //stairs.Add("C04.54");

        elevators.Add("C04.Elevator_1");
        elevators.Add("C04.Elevator_2");
        //elevators.Add("C04.55");

        coffeeSpots.Add("C04.Coffee");

        printers.Add("C04.Printer");

        lockers.Add("C04.Lockers");
    }


    // Update is called once per frame
    void Update()
    {
        // Check if the dropdown is expanded (i.e., opened)
        if (destinations.IsExpanded && !isDropdownOpen)
        {
            // Dropdown is opened
            OnDropdownOpened();
            isDropdownOpen = true;
        }
        else if (!destinations.IsExpanded && isDropdownOpen)
        {
            // Dropdown is closed
            isDropdownOpen = false;
        }
    }

    // Method to be called when the dropdown menu is opened
    void OnDropdownOpened()
    {
        for (int i = 1; i < destinations.options.Count; i++)
        {
            {
                Transform extraLabelTransform = destinations.transform.GetChild(4).GetChild(0).GetChild(0).GetChild(i + 1).GetChild(2);
                TextMeshProUGUI extraLabel = extraLabelTransform.GetComponent<TextMeshProUGUI>();
                extraLabel.text = estimateData[destinations.options[i].text].ToString("F0") + " meters away";
            }
        }
        // Add your code to handle the dropdown opening here
    }

    public void ClearFilters()
    {
        typeDropdown.value = 0;
        typeDropdown.RefreshShownValue();
        blockDropdown.value = 0;
        blockDropdown.RefreshShownValue();
        floorDropdown.value = 0;
        floorDropdown.RefreshShownValue();
    }

    public void ApplyFilters()
    {
        destinations.ClearOptions();
        filteredOptions.Clear();
        if (typeDropdown.value == 0)
        {
            filteredOptions.AddRange(classrooms);
            filteredOptions.AddRange(groupRooms);
            filteredOptions.AddRange(wc);
            filteredOptions.AddRange(wcHandicapped);
            filteredOptions.AddRange(stairs);
            filteredOptions.AddRange(elevators);
            filteredOptions.AddRange(coffeeSpots);
            filteredOptions.AddRange(printers);
            filteredOptions.AddRange(lockers);
        }
        else
        {
            filteredOptions.AddRange(listDictionary[typeDropdown.options[typeDropdown.value].text]);
        }
        
        // New filtering using a dropdown menu
        for (int i = filteredOptions.Count - 1; i >= 0; i--)
        {
            if (!filteredOptions[i].StartsWith(blockDropdown.captionText.text))
            {
                filteredOptions.RemoveAt(i);
            }
            else if (filteredOptions[i][2] != floorDropdown.captionText.text[0])
            {
                filteredOptions.RemoveAt(i);
            }
        }
        
        
        filteredOptions.Sort();
        destinations.options.Insert(0, new TMP_Dropdown.OptionData("Choose Location"));
        destinations.AddOptions(filteredOptions);
    }
}
