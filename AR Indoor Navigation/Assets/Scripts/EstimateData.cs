using UnityEngine;
using System.Collections.Generic;

public class EstimateData : MonoBehaviour
{
    private Dictionary<string, float> collectedEstimates;
    private Dictionary<string, string> allTargets = new Dictionary<string, string>();
    public GameObject targets;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        foreach (Transform child in targets.transform)
        {
            foreach (Transform grandChild in child)
            {
                allTargets.Add(grandChild.gameObject.name, grandChild.gameObject.tag);
                Debug.Log("target: " + grandChild.gameObject.name + ", its tag: " + grandChild.gameObject.tag);
            }
        }
    }
    public void CollectNewEstimates(Dictionary<string, float> locations)
    {
        collectedEstimates = new Dictionary<string, float>(locations);
        WriteEstimates();
    }
    public Dictionary<string, float> getCurrentEstimates()
    {
        return collectedEstimates;
    }

    public void WriteEstimates()
    {
        foreach (KeyValuePair<string, float> pair in collectedEstimates)
        {
            Debug.Log("Location: " + pair.Key + ", Length: " + pair.Value);
        }
    }
}