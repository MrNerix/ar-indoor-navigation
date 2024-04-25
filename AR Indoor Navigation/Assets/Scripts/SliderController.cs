using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderController : MonoBehaviour
{
    public TMP_Text chosenValue;
    public void onSliderChanged(float value)
    {
        chosenValue.text = value.ToString();
    }
}
