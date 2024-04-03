using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DropDown : MonoBehaviour
{
    [Serializefield] private TMP_Dropdown dropdown;
    // Start is called before the first frame update
    
    public void DropDownFeature()
    {
        int pickedEntryIntex = dropdown.value;

        Debug.Log(pickedEntryIntex);
    }
}
