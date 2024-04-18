using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TargetValue : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown navigationTargetDropDown;

    public string selectedText;
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetTarget(int selectedValue)
    {
        selectedText = navigationTargetDropDown.options[selectedValue].text;
        SceneManager.LoadScene("Main_Scene");
    }
    public string GetTargetedText()
    {
        return selectedText;
    }
}
