using UnityEngine;

public class ResetPos : MonoBehaviour
{
    //public Vector3Data resetPos;
    public Vector3 resetPos;
    public Quaternion resetPosRot;

    public void ResetPlayerLocation()
    {
        // Set the player's position to the new position
        transform.position = resetPos;
        transform.rotation = resetPosRot;
        
        Debug.Log("resetposValue: " + resetPos);
    }
}
