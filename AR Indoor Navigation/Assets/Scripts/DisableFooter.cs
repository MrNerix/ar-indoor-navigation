using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableFooter : MonoBehaviour
{
    public GameObject footerExpanded;
    public GameObject footerSmall;

    public void Start()
    {
        footerSmall.SetActive(false);
        footerExpanded.SetActive(true);
    }
    
    public void CompressButton()
    {
        if (footerExpanded.activeInHierarchy == true)
        {
            footerExpanded.SetActive(false);
            footerSmall.SetActive(true);
        }
    }

    public void ShowMiniMap()
    {
        footerExpanded.SetActive(true);
        footerSmall.SetActive(false);
    }
}
