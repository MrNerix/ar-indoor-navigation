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
        DontDestroyOnLoad(gameObject);
    }

    public void SceneChange(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void SetTarget()
    {
        selectedText = navigationTargetDropDown.options[navigationTargetDropDown.value].text;
        SceneManager.LoadScene("Main_Scene");
    }
    public string GetTargetedText()
    {
        return selectedText;
    }
}
