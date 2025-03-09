using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;  // Assign your character here
    public Vector3 offset = new Vector3(0, 5, -10); // Adjust for desired view
    public float smoothSpeed = 5f; // Adjust for smooth movement

    void LateUpdate()
    {
        if (target == null) return;

        // Desired position with offset
        Vector3 desiredPosition = target.position + offset;


        // Smooth transition using Lerp
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Optional: Make the camera look at the player
        transform.LookAt(target);
    }
}