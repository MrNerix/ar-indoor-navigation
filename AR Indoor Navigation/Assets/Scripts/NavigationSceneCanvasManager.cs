using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationSceneCanvasManager : MonoBehaviour
{
    public GameObject footerExpanded;
    public GameObject footerSmall;
    public GameObject qrScanner;

    public void Start()
    {
        footerSmall.SetActive(false);
        footerExpanded.SetActive(false);
        qrScanner.SetActive(true);
    }

    public void DisableQRScannerCanver()
    {
        qrScanner.SetActive(false);
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
