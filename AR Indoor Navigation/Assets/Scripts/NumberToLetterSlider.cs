using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NumberToLetterSlider : MonoBehaviour
{
    [SerializeField] private Slider slider = null;
    [SerializeField] private TMP_Text sliderValue = null;
    [SerializeField] private List<string> charValues = new List<string> { "A", "B", "C", "D" };

    private void Start()
    {
        slider.onValueChanged.AddListener(delegate { SliderValueChangedCallback(); });
        slider.minValue = 0;
        slider.maxValue = charValues.Count - 1;
        slider.wholeNumbers = true;
        
    }
    
    private void SliderValueChangedCallback()
    {
        int numericSliderValue = (int)slider.value;
        sliderValue.text = charValues[numericSliderValue];
    }
}
