using UnityEngine;

public class BedLock : MonoBehaviour
{
    public bool isLocked = true;
    public Transform spawnLocation;

    public void Unlock()
    {
        isLocked = false;
        gameObject.SetActive(false);
    }
    
    public void SetBedLocator(Transform locator)
    {
        var bedLocator = locator;
        // You can perform any additional logic related to the locator here
        Debug.Log("Bed locator assigned: " + bedLocator.name);
    }
}