using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationSceneCanvasManager : MonoBehaviour
{
    public GameObject footerExpanded;
    public GameObject footerSmall;
    public GameObject qrScanner;

    private void Start()
    {
        footerSmall.SetActive(false);
        footerExpanded.SetActive(true);
        qrScanner.SetActive(false);
    }

    public void DisableQRScannerCanvas()
    {
        qrScanner.SetActive(false);
    }

    public void EnableFooter()
    {
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
