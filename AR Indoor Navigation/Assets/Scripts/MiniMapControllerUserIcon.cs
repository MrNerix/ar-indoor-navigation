using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapControllerUserIcon : MonoBehaviour
{
    public RectTransform miniMapRectTransform; // Assign in the inspector
    public RectTransform userIconRectTransform; // Assign in the inspector

    // Assuming these are the bounds of the mini-map in real-world coordinates
    public float mapRealWorldWidthInMeters; // The real-world width covered by the mini-map.
    public float mapRealWorldHeightInMeters; // The real-world height covered by the mini-map.

    private float x = 0;
    private bool xBool = true;
    private float y = 0;
    private bool yBool = true;

    // Update is called once per frame
    void Update()
    {
        // Example user position in real-world coordinates
        Vector2 userRealWorldPosition = GetUserRealWorldPosition();

        // Convert real-world position to mini-map position
        Vector2 userOnMiniMapPosition = ConvertRealWorldPositionToUserOnMiniMapPosition(userRealWorldPosition);

        // Update the user icon's position on the mini-map
        userIconRectTransform.anchoredPosition = userOnMiniMapPosition;
    }

    Vector2 ConvertRealWorldPositionToUserOnMiniMapPosition(Vector2 realWorldPosition)
    {
        // Calculate the position as a percentage of the real-world bounds
        float xPercentage = realWorldPosition.x / mapRealWorldWidthInMeters;
        float yPercentage = realWorldPosition.y / mapRealWorldHeightInMeters;

        // Convert percentages to position within the mini-map RectTransform
        float userOnMiniMapPositionX = xPercentage * miniMapRectTransform.sizeDelta.x;
        float userOnMiniMapPositionY = yPercentage * miniMapRectTransform.sizeDelta.y;

        return new Vector2(userOnMiniMapPositionX - miniMapRectTransform.sizeDelta.x / 2, userOnMiniMapPositionY - miniMapRectTransform.sizeDelta.y / 2);
    }

    Vector2 GetUserRealWorldPosition()
    {
        SimulateUserMovementX();
        SimulateUserMovementY();


        // Placeholder for getting the user's real-world position
        // This should be replaced with your method of obtaining the user's position
        return new Vector2(13f + x,1.0f +y);
        //return new Vector2(500, 500); // Example position
    }

    void SimulateUserMovementX() {
        if (xBool) {
            x = x + 0.03f;
            if (x > 1f) xBool = false;
        } else {
            x = x - 0.05f;
            if (x < 0f) xBool = true;
        }
    }

    void SimulateUserMovementY() {
        if (yBool) {
            y = y + 0.03f;
            if (y > 20f) yBool = false;
        } else {
            y = y - 0.05f;
            if (y < 0f) yBool = true;
        }
    }
}