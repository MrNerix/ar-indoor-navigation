using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown navigationTargetDropDown;

    public string selectedText;
    // Start is called before the first frame update
    void Awake()
    {
        if (gameObject.name == "NavigationManager")
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SceneChange(int index)
    {
        SceneManager.LoadScene(index);
    }
    // To Main Menu (where filters and choose destination is) from Main Scene (navigation) 
    public void ToSelection()
    {
        GameObject OldNavManager = GameObject.Find("NavigationManager");

        // Check if the GameObject exists
        if (OldNavManager != null)
        {
            // If it exists, destroy it
            Destroy(OldNavManager);
        }
        SceneManager.LoadScene("Main_Menu");
    }
    // To Main Scene (navigation) from Main Menu (where filters and choose destination is)
    public void ToNavigation()
    {
        selectedText = navigationTargetDropDown.options[navigationTargetDropDown.value].text;
        SceneManager.LoadScene("Navigation");
    }
    public string GetTargetedText()
    {
        return selectedText;
    }
}