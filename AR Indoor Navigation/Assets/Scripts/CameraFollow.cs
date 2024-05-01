using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Assign this in the Inspector
    public float height = 10f; // Height above the target

    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = new Vector3(target.position.x, height, target.position.z);
        }
    }
}