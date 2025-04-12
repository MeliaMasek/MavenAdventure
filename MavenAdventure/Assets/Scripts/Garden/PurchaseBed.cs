using UnityEngine;

public class PurchaseBed : MonoBehaviour
{
    [Header("Bed Prefabs")]
    public GameObject lockedBedPrefab; // Reference to the locked bed prefab
    public GameObject unlockedBedPrefab; // Reference to the unlocked bed prefab

    [Header("Purchase Settings")]
    public int bedCost; // Cost to unlock the bed
    //public UnityEngine.UI.Text coinText; // UI Text to display player's coins
    public IntData playerCoins;

    private bool isBedUnlocked = false;

    public GameObject unlockMenuUI; // Your confirm panel UI

    public GameObject[] bedPrefabs; // Array to hold different bed prefabs
    public Transform[] spawnLocations; // Array to hold spawn locations (locators)

    private GameObject currentBedInstance;
    public int spawnIndex = 0; // Replace with your desired index
    public Vector3 spawnScale;


    void Start()
    {
        // Load player's coin count and bed purchase status
        isBedUnlocked = PlayerPrefs.GetInt("IsBedUnlocked", 0) == 1;

        // Initialize the bed's state
        //InitializeBedState();
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

                    if (unlockMenuUI != null)
                    {
                        unlockMenuUI.SetActive(true);
                    }
                    else
                    {
                        Debug.LogWarning("UnlockedUI is not assigned.");
                    }
                }
            }
        }
    }

    private void OnMouseDown()
    {
        // Optional: Add logic to make sure this only runs when unlocked == false
        if (unlockMenuUI != null)
        {
            unlockMenuUI.SetActive(true);
        }
    }

    void InitializeBedState()
    {
        if (isBedUnlocked)
        {
            currentBedInstance = Instantiate(unlockedBedPrefab, transform.position, transform.rotation, transform);
        }
        else
        {
            currentBedInstance = Instantiate(lockedBedPrefab, transform.position, transform.rotation, transform);
        }
    }

    public void AttemptPurchase(int bedIndex, int spawnIndex)
    {
        if (playerCoins.value >= bedCost)
        {
            playerCoins.value -= bedCost;

            if (bedIndex >= 0 && bedIndex < bedPrefabs.Length && spawnIndex >= 0 && spawnIndex < spawnLocations.Length)
            {
                Transform spawnLocation = spawnLocations[spawnIndex];
                GameObject bedPrefab = bedPrefabs[bedIndex];

                // Instantiate the selected bed prefab at the chosen spawn location
                Instantiate(bedPrefab, spawnLocation.position, spawnLocation.rotation);
            }
            else
            {
                Debug.LogError("Invalid bed or spawn location index.");
            }
        }
        else
        {
            Debug.Log("Not enough coins to purchase the bed.");
        }
    }
    private void ReplaceBedPrefab(GameObject newPrefab, Transform spawnLocation = null, Vector3? customScale = null)
    {
        // Destroy the current bed instance if it exists
        if (currentBedInstance != null)
        {
            Destroy(currentBedInstance);
            currentBedInstance = null;
        }

        // Determine the position and rotation for the new bed
        Vector3 position = spawnLocation != null ? spawnLocation.position : Vector3.zero;
        Quaternion rotation = spawnLocation != null ? spawnLocation.rotation : Quaternion.identity;

        // Instantiate the new bed prefab
        currentBedInstance = Instantiate(newPrefab, position, rotation);

        // Apply custom scale if provided
        if (customScale.HasValue)
        {
            currentBedInstance.transform.localScale = customScale.Value;
        }
    }
    
    
    public void OnPurchaseConfirmed()
    {
        if (playerCoins.value >= bedCost)
        {
            playerCoins.value -= bedCost;

            isBedUnlocked = true;
            PlayerPrefs.SetInt("IsBedUnlocked", 1);
            PlayerPrefs.Save();
            
            Vector3 desiredScale = new Vector3(.75f, .75f, .75f);
            ReplaceBedPrefab(unlockedBedPrefab, spawnLocations[spawnIndex], desiredScale);
            if (unlockMenuUI != null)
            {
                unlockMenuUI.SetActive(false);
            }
        }
        else
        {
            Debug.Log("Not enough coins to purchase the bed.");
        }
    }
}