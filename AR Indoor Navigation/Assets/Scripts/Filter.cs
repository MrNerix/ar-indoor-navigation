using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Filter : MonoBehaviour
{

    public Slider blockSlider;
    public Slider floorSlider;
    public TMP_Dropdown typeDropdown;
    public TMP_Text chosenBlock;
    public TMP_Text chosenFloor;

    // Lists (used before we implement a database)

    public Dictionary<string, List<string>> listDictionary = new Dictionary<string, List<string>>();
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

    public void ClearFilters()
    {
        blockSlider.value = 0f;
        floorSlider.value = 0f;
        typeDropdown.value = 0;
        typeDropdown.RefreshShownValue();
    }

    public void ApplyFilters()
    {
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
            Debug.Log(filteredOptions.Count);
        }
        else
        {
            filteredOptions.AddRange(listDictionary[typeDropdown.options[typeDropdown.value].text]);
        }
    }
}
