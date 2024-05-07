using UnityEngine;
using System.Collections.Generic;

public class EstimateData : MonoBehaviour
{
    private Dictionary<string, float> collectedEstimates;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
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