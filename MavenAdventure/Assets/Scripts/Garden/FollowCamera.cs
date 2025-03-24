using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;  
    public Vector3 offset3D = new Vector3(0, 5, -10); // Default 3D offset
    public Vector3 offset2D = new Vector3(0, 10, 0); // Top-down 2D offset
    public float smoothSpeed = 5f; 

    public Vector3 lookAtOffset = new Vector3(0, 1.5f, 0); // Adjust where the camera aims
    private bool is2DMode = false;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = is2DMode ? target.position + offset2D : target.position + offset3D;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        if (is2DMode)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(90f, 0f, 0f), smoothSpeed * Time.deltaTime);
        }
        else
        {
            Vector3 lookAtTarget = target.position + lookAtOffset;
            transform.LookAt(lookAtTarget);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CameraSwitchZone"))
        {
            is2DMode = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CameraSwitchZone"))
        {
            is2DMode = false;
        }
    }
}