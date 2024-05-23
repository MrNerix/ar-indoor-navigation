using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Filter : MonoBehaviour
{

    public Slider blockSlider;
    public Slider floorSlider;
    public TMP_Dropdown typeDropdown;
    public TMP_Dropdown destinations;
    public TMP_Text chosenBlock;
    public TMP_Text chosenFloor;
    private bool isDropdownOpen = false;
    private GameObject est;
    private EstimateData estimates;

    public Dictionary<string, float> estimateData = new Dictionary<string, float>();
    public Dictionary<string, string> listDictionary = new Dictionary<string, string>();
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

        foreach (KeyValuePair<string, string> kvp in estimates.getAllDestinations())
        {
            listDictionary.Add(kvp.Key, kvp.Value);
            filteredOptions.Add(kvp.Key);
        }
        destinations.ClearOptions();
        filteredOptions.Sort();
        destinations.AddOptions(filteredOptions);
        destinations.options.Insert(0, new TMP_Dropdown.OptionData("Choose Location"));
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
                if (!estimateData.ContainsKey(destinations.options[i].text))
                {
                    extraLabel.text = "destination is in a different block / floor";
                }
                else
                {
                    extraLabel.text = estimateData[destinations.options[i].text].ToString("F0") + " meters away";
                }
            }
        }
    }

    public void ClearFilters()
    {
        blockSlider.value = 0f;
        floorSlider.value = 0f;
        typeDropdown.value = 0;
        typeDropdown.RefreshShownValue();
    }

    public void ApplyFilters()
    {
        destinations.ClearOptions();
        filteredOptions.Clear();
        if (typeDropdown.value == 0)
        {
            foreach (KeyValuePair<string, string> kvp in listDictionary)
            {
                filteredOptions.Add(kvp.Key);
            }
        }
        else
        {
            foreach (KeyValuePair<string, string> kvp in listDictionary)
            {
                if (kvp.Value == typeDropdown.options[typeDropdown.value].text)
                {
                    filteredOptions.Add(kvp.Key);
                }
            }
        }

        for (int i = filteredOptions.Count - 1; i >= 0; i--)
        {
            if (!filteredOptions[i].StartsWith(chosenBlock.text))
            {
                filteredOptions.RemoveAt(i);
            }
            else if (filteredOptions[i][2] != chosenFloor.text[0])
            {
                filteredOptions.RemoveAt(i);
            }
        }
        filteredOptions.Sort();
        destinations.options.Insert(0, new TMP_Dropdown.OptionData("Choose Location"));
        destinations.AddOptions(filteredOptions);
    }
}
