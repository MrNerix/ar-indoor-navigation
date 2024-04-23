using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class CanvasLightCameraManager : MonoBehaviour
{

    public Canvas miniMapCanvas;
    public Canvas mapOverviewCanvas;
    public Light directionalLight;
    // Start is called before the first frame update
    void Start()
    {
        ShowARModeCanvas();        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowARModeCanvas()
{
    miniMapCanvas.enabled = true;
    mapOverviewCanvas.enabled = false;
    directionalLight.intensity = 1.0f;
}

public void ShowMapOverviewCanvas()
{
    miniMapCanvas.enabled = false;
    mapOverviewCanvas.enabled = true;
    directionalLight.intensity = 0.8f;
}
}
