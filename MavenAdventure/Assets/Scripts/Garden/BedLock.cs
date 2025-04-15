using UnityEngine;
//Code created, borrowed, and modified from Chat GPT

public class BedLock : MonoBehaviour
{
    public bool isLocked = true;
    public Transform spawnLocation;
    public PurchaseBed purchaseBed;  // Assign this on spawn

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
    
    private void OnMouseDown()
    {
        if (!isLocked) return;

        if (purchaseBed != null && purchaseBed.IsPlayerInRange(transform))
        {
            purchaseBed.ShowUnlockMenu();
        }
        else
        {
            Debug.Log("Player too far away or Purchase Manager not assigned.");
        }
    }
}