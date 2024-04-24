using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Filter : MonoBehaviour
{
    // Lists (used before we implement a database)
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
}
