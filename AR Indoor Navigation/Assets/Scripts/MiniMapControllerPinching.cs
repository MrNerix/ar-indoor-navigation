using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MiniMapControllerPinching : MonoBehaviour, IDragHandler, IPointerDownHandler, IScrollHandler
{
    private Vector2 lastPanPosition;
    private int panFingerId; // Touch mode only

    private float zoomSpeedTouch = 0.001f;
    private float[] bounds = new float[] { 0.3f, 3f }; // Min and Max Zoom
    private RectTransform rectTransform;
    private RectTransform initialRectTransform;

    //TODO: task - the image with the map shall snap back to en edge when dragged from.
    // private bool isPinching = false;
    // private int[] isPinchingCounter = {0,0};
    // private Vector2 initialMapAnchoredPosition;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        //TODO: task - the image with the map shall snap back to en edge when dragged from. 
        // initialMapAnchoredPosition = rectTransform.anchoredPosition;
    }

    // Dragging the image
    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.pointerId < -1 || eventData.pointerId == panFingerId)
        {
            Vector2 currentPanPosition = eventData.position;
            Vector2 delta = currentPanPosition - lastPanPosition;
            rectTransform.anchoredPosition += delta;
            lastPanPosition = currentPanPosition;
        }
    }

    // Dragging the image
    public void OnPointerDown(PointerEventData eventData)
    {
        lastPanPosition = eventData.position;
        panFingerId = eventData.pointerId;
    }

    // Scrolling the image
    public void OnScroll(PointerEventData data)
    {
        Vector3 scale = rectTransform.localScale;
        scale += Vector3.one * (data.scrollDelta.y * zoomSpeedTouch);
        scale.x = Mathf.Clamp(scale.x, bounds[0], bounds[1]);
        scale.y = Mathf.Clamp(scale.y, bounds[0], bounds[1]);
        rectTransform.localScale = scale;
    }

    // Pinching the image
    private void Zoom(float increment)
    {
        Vector3 scale = rectTransform.localScale;
        scale += Vector3.one * increment;
        scale.x = Mathf.Clamp(scale.x, bounds[0], bounds[1]);
        scale.y = Mathf.Clamp(scale.y, bounds[0], bounds[1]);
        rectTransform.localScale = scale;
        
        //TODO: task - the image with the map shall snap back to en edge when dragged from.
        //transform.position = new Vector3(transform.position.x -1, transform.position.y, transform.position.z);  
        //Debug.Log("AAA: " + rectTransform.anchoredPosition);
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
        //TODO: task - the image with the map shall snap back to en edge when dragged from.
        //MapImagePositionCheck(); 
    }
    void MapImagePositionCheck() {
        //TODO: task - the image with the map shall snap back to en edge when dragged from.
        //float scale = rectTransform.localScale.x;

        // if (rectTransform.anchoredPosition.x*scale > initialMapAnchoredPosition.x*scale) {
        //     transform.position = new Vector3(transform.position.x -1, transform.position.y, transform.position.z);  
        //     Debug.Log("AAA: " + rectTransform.anchoredPosition);
        // } 


        //     if (isPinching) {

        //     x = x + 0.03f;
        //     if (x > 1f) isPinching = false;
        // } else {
        //     x = x - 0.05f;
        //     if (x < 0f) isPinching = true;
    }
}
