using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapOverviewPinching : MonoBehaviour, IDragHandler, IPointerDownHandler, IScrollHandler
{
    public Camera topDownCameraFar;
    private Vector2 lastPanPosition;
    private int panFingerId; // Touch mode only

    private float zoomSpeedTouch = 0.001f;
    private float[] bounds = new float[] { 5f, 100f }; // Min and Max Zoom
    private RectTransform rectTransform;
    private RectTransform initialRectTransform;

    void Awake()
    {
       rectTransform = GetComponent<RectTransform>();
    }
    public void OnDrag(PointerEventData eventData)
    {
        // TODO: change the fucntion behaviour so that it doesnt change position of image, but rather of the camera
        if (eventData.pointerId < -1 || eventData.pointerId == panFingerId)
        {
            Vector2 currentPanPosition = eventData.position;
            Vector2 delta = currentPanPosition - lastPanPosition;
            rectTransform.anchoredPosition += delta;
            lastPanPosition = currentPanPosition;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        lastPanPosition = eventData.position;
        panFingerId = eventData.pointerId;
    }

    public void OnScroll(PointerEventData eventData)
    {
        Vector3 scale = rectTransform.localScale;
        scale += Vector3.one * (eventData.scrollDelta.y * zoomSpeedTouch);
        scale.x = Mathf.Clamp(scale.x, bounds[0], bounds[1]);
        scale.y = Mathf.Clamp(scale.y, bounds[0], bounds[1]);
        rectTransform.localScale = scale;
    }

   private void Zoom(float increment)
    {
        increment = increment * -1;
        float fieldOfView =topDownCameraFar.fieldOfView + topDownCameraFar.fieldOfView * increment;
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

   



