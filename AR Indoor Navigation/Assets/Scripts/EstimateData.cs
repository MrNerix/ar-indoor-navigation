using UnityEngine;
using System.Collections.Generic;

public class EstimateData : MonoBehaviour
{
    // Field to store the collected estimates
    private Dictionary<string, float> collectedEstimates;

    // Function to collect estimates
    public void CollectEstimates(Dictionary<string, float> locations)
    {
        // Store the received data in the collectedEstimates field
        collectedEstimates = new Dictionary<string, float>(locations);

        // Optionally, you can perform additional processing or use the data as needed
        foreach (KeyValuePair<string, float> pair in collectedEstimates)
        {
            Debug.Log("Location: " + pair.Key + ", Length: " + pair.Value);
        }
    }
}