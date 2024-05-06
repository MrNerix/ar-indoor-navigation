using UnityEngine;
using System.Collections.Generic;

public class EstimateData : MonoBehaviour
{
    private Dictionary<string, float> collectedEstimates;

    public void CollectEstimates(Dictionary<string, float> locations)
    {
        collectedEstimates = new Dictionary<string, float>(locations);
        foreach (KeyValuePair<string, float> pair in collectedEstimates)
        {
            Debug.Log("(ED)Location: " + pair.Key + ", Length: " + pair.Value);
        }
    }
}