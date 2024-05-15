using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapOverviewPinching : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    public Camera topDownCameraFar;
    private Vector2 lastPanPosition;
    private int panFingerId; // Touch mode only

    private float zoomSpeedTouch = 0.001f;
    private float[] bounds = new float[] { 5f, 100f }; // Min and Max Zoom
    private Transform transformCam;

    void Awake()
    {
           transformCam = topDownCameraFar.transform;
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.pointerId < -1 || eventData.pointerId == panFingerId)
        {
            Vector2 currentPanPosition = eventData.position;
            Vector2 delta = currentPanPosition - lastPanPosition;
            float newX = transformCam.position.x + delta.x/20;
            float newZ = transformCam.position.z + delta.y/20;
            float newY = transformCam.position.y;
            Vector3 newPosition = new Vector3(newX, newY, newZ);
        
            // Apply the movement
            transformCam.position = newPosition;
            // Debug.Log("x " + newPosition.x + " | y " + newPosition.y  + " | z " + newPosition.z);
            lastPanPosition = currentPanPosition;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        lastPanPosition = eventData.position;
        panFingerId = eventData.pointerId;
    }

   private void Zoom(float increment)
    {
        increment = increment * -1;
        float fieldOfView = topDownCameraFar.fieldOfView + topDownCameraFar.fieldOfView * increment;
        fieldOfView = Mathf.Clamp(fieldOfView, bounds[0], bounds[1]);
        topDownCameraFar.fieldOfView = fieldOfView;
        // Debug.Log("AAA: " + diff);
    }

    void Update()
    {
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            Zoom(difference * zoomSpeedTouch);
        }
    }
}

   



