using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;  
    public Vector3 offset = new Vector3(0, 5, -10); // Adjust for desired view
    public float smoothSpeed = 5f; 

    public Vector3 lookAtOffset = new Vector3(0, 1.5f, 0); // Adjust where the camera aims

    void LateUpdate()
    {
        if (target == null) return;

        // Desired position with offset
        Vector3 desiredPosition = target.position + offset;

        // Smooth transition using Lerp
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Adjust the LookAt target to a higher position (e.g., the head)
        Vector3 lookAtTarget = target.position + lookAtOffset;
        transform.LookAt(lookAtTarget);
    }
}
