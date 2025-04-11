using UnityEngine;
using UnityEngine.Events;

public class PurchaseBed : MonoBehaviour
{
    [Header("Unlock Settings")]
    public GameObject lockedObject;         // The current locked version (e.g., a faded-out bed)
    public GameObject unlockedPrefab;       // The prefab to replace it with
    public IntData playerCoins;             // Your ScriptableObject holding coin value
    public int unlockCost = 10;
    public GameObject unlockMenuUI; // Your confirm panel UI
    public GameObject UnlockedUI;
    public PlantManager plantManager; // Reference to the PlantManager


    [Header("Optional")]
    public UnityEvent onUnlocked;           // Hook into other systems (like sound, animation, etc.)

    private bool isUnlocked = false;

    public void AttemptUnlock()
    {
        Debug.Log("AttemptUnlock called!");

        if (playerCoins.value >= unlockCost)
        {
            playerCoins.value -= unlockCost;

            if (plantManager != null)
            {
                // Replace the locked object with the unlocked prefab
                plantManager.ReplaceWithUnlocked(unlockedPrefab, lockedObject);
            }
            else
            {
                Debug.LogWarning("No spawner assigned!");
            }
        }
        else
        {
            Debug.Log("Not enough coins!");
        }
    }

    private void UnlockObject()
    {
        isUnlocked = true;

        // Get the current position and rotation of the locked object
        Vector3 position = lockedObject.transform.position;
        Quaternion rotation = lockedObject.transform.rotation;

        // Destroy the locked object instance (not the prefab)
        Destroy(lockedObject); // lockedObject is an instance, not the prefab

        // Instantiate the unlocked prefab in the same position
        GameObject newObj = Instantiate(unlockedPrefab, position, rotation);

        // Optional: fire any events
        onUnlocked?.Invoke();
    }
    
    private void OnMouseDown()
    {
        // Optional: Add logic to make sure this only runs when unlocked == false
        if (unlockMenuUI != null)
        {
            unlockMenuUI.SetActive(true);
        }
    }
   private void Start()
    {
        // Check and assign the PurchaseBedUI at runtime
        if (UnlockedUI == null)
        {
            // Search for the inactive UI object by its name
            GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
            foreach (var obj in allObjects)
            {
                if (obj.name == "UnlockPanel") // Replace with the actual name of your panel GameObject
                {
                    UnlockedUI = obj;
                    break;
                }
            }
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button click
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("LockedBed"))
                {
                    Debug.Log("Clicked on the locked bed: " + hit.collider.gameObject.name);

                    if (UnlockedUI != null)
                    {
                        UnlockedUI.SetActive(true);
                    }
                    else
                    {
                        Debug.LogWarning("UnlockedUI is not assigned.");
                    }
                }
            }
        }
    }
}