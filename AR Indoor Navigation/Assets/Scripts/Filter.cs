using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Filter : MonoBehaviour
{
    // Lists (used before we implement a database)
    public List<string> classrooms = new List<string>();
    public List<string> groupRooms = new List<string>();
    public List<string> wc = new List<string>();
    public List<string> wcHandicapped = new List<string>();
    public List<string> stairs = new List<string>();
    public List<string> elevators = new List<string>();
    public List<string> coffeeSpots = new List<string>();
    public List<string> printers = new List<string>();
    public List<string> lockers = new List<string>();
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

        wc.Add("C04.60");
        wc.Add("C04.61");

        wcHandicapped.Add("C04.63");

        stairs.Add("C04.50");
        stairs.Add("C04.52");
        stairs.Add("C04.54");

        elevators.Add("C04.51");
        elevators.Add("C04.53");
        elevators.Add("C04.55");

        coffeeSpots.Add("C04.Coffee");

        printers.Add("C04.Print");

        lockers.Add("C04.Lockers");
    }
}
