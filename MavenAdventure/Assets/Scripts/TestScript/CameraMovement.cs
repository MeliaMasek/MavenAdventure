using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public Vector3 offset; // Offset of the camera from the player
    public float rotationSpeed = 5f; // Speed at which the camera rotation catches up to the player's rotation

    private Quaternion targetRotation; // The rotation the camera is trying to reach

    void LateUpdate()
    {
        if (player != null)
        {
            // Set the camera's position to the player's position plus the offset
            transform.position = player.position + offset;

            // Calculate the rotation that the camera should have based on the player's rotation
            Quaternion desiredRotation = Quaternion.LookRotation(player.position - transform.position, Vector3.up);

            // Smoothly interpolate between the current rotation and the desired rotation
            targetRotation = Quaternion.Slerp(targetRotation, desiredRotation, Time.deltaTime * rotationSpeed);

            // Apply the rotation to the camera
            transform.rotation = targetRotation;
        }
    }
}