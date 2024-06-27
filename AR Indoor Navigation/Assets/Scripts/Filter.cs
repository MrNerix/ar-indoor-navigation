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

    public Sprite favoriteIconAdd;
    public GameObject favIcon;


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

        HandleFavourites();
    }

    private void HandleFavourites()
    {
        AddEmptyHeartToAllOptions(); // Adding a favourite icon to all the destination options
        // Initial loading favourites from a file
        favIcon.GetComponent<FavIconHandler>().LoadFavouritesOptionsFromFile();
    }

    private void AddEmptyHeartToAllOptions()
    {
        TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
        for (int i = 1; i < destinations.options.Count; i++)
        {
            option = destinations.options[i];
            //option.text = "" + option.text + "AAA";
            option.image = favoriteIconAdd;
            destinations.options.RemoveAt(i);
            destinations.options.Insert(i, option);
        }
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

        //Type filtering
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

        // Block filtering
        if (blockDropdown.value != 0)
        {
            for (int i = filteredOptions.Count - 1; i >= 0; i--)
            {
                if (!filteredOptions[i].StartsWith(blockDropdown.captionText.text))
                {
                    filteredOptions.RemoveAt(i);
                }
            }
        }
        // Floor filtering
        if (floorDropdown.value != 0)
        {
            for (int i = filteredOptions.Count - 1; i >= 0; i--)
            {
                if (filteredOptions[i][2] != floorDropdown.captionText.text[0])
                {
                    filteredOptions.RemoveAt(i);
                }
            }
        }


        filteredOptions.Sort();
        destinations.options.Insert(0, new TMP_Dropdown.OptionData("Choose Location"));
        destinations.AddOptions(filteredOptions);
        HandleFavourites();
    }
}
