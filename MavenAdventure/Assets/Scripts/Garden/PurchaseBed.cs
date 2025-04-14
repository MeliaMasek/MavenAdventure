using System.Collections.Generic;
using UnityEngine;

public class PurchaseBed : MonoBehaviour
{
    [Header("Bed Prefabs")] public GameObject lockedBedPrefab; // Reference to the locked bed prefab
    public GameObject unlockedBedPrefab; // Reference to the unlocked bed prefab

    [Header("Purchase Settings")] public int bedCost; // Cost to unlock the bed
    public IntData playerCoins; // Reference to the player's coin data

    private bool isBedUnlocked = false;

    public GameObject unlockMenuUI; // Your confirm panel UI
    public GameObject[] bedPrefabs; // Array to hold different bed prefabs
    public List<Transform> spawnLocations;

    private GameObject currentBedInstance;
    public int spawnIndex = 0; // Replace with your desired index
    public Vector3 spawnScale;

    public PlantManager plantManager;
    private InventoryData plantData;
    
    public float interactionDistance = 2f;
    public Transform player;
    
    

    void Start()
    {
        // Initialize the PlantManager reference
        plantManager = FindObjectOfType<PlantManager>();

        // Load player's coin count and bed purchase status
        isBedUnlocked = PlayerPrefs.GetInt("IsBedUnlocked", 0) == 1;

        // Initialize the bed's state
        //InitializeBedState();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))  // Mobile taps register as mouse 0
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == this.gameObject)
                {
                    if (IsPlayerInRange())
                    {
                        Debug.Log("Clicked on bed within range!");

                        if (unlockMenuUI != null)
                        {
                            unlockMenuUI.SetActive(true);
                        }
                        else
                        {
                            Debug.LogWarning("UnlockMenuUI is not assigned.");
                        }
                    }
                    else
                    {
                        Debug.Log("Player too far away to interact!");
                    }
                }
            }
        }
    }

    private bool IsPlayerInRange()
    {
        if (player == null)
        {
            Debug.LogError("Player reference not assigned!");
            return false;
        }

        float distance = Vector3.Distance(transform.position, player.position);
        Debug.Log("Distance to player: " + distance);
        return distance <= interactionDistance;
    }
    
    private void OnMouseDown()
    {
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

            if (bedIndex >= 0 && bedIndex < bedPrefabs.Length && spawnIndex >= 0 && spawnIndex < spawnLocations.Count)
            {
                Transform spawnLocation = spawnLocations[spawnIndex];
                GameObject bedPrefab = bedPrefabs[bedIndex];

                // Instantiate the selected bed prefab at the chosen spawn location
                GameObject newBed = Instantiate(bedPrefab, spawnLocation.position, spawnLocation.rotation);
                BedLock bedLock = newBed.GetComponent<BedLock>();
                if (bedLock != null)
                {
                    bedLock.purchaseBed = this;  // THIS assigns the manager!
                }
                // Unlock the bed in the PlantManager
                UnlockBedInPlantManager(spawnLocation);
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

    private void UnlockBedInPlantManager(Transform spawnLocation)
    {
        foreach (var plant in plantManager.plants)
        {
            if (plant.spawnLocator == spawnLocation)
            {
                plant.isBedLocked = false;
                plant.baseStageObject.SetActive(true);
                Debug.Log("Bed unlocked in PlantManager.");
                break;
            }
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
        //currentBedInstance = Instantiate(newPrefab, position, rotation);

        // Apply custom scale if provided
        if (customScale.HasValue)
        {
            //currentBedInstance.transform.localScale = customScale.Value;
        }
    }

    public void OnPurchaseConfirmed()
    {
        if (playerCoins.value >= bedCost)
        {
            // Assuming spawnLocations is a List<Transform>
            foreach (Transform spawnLocator in spawnLocations)
            {
                // Create a list with a single spawn location
                List<Transform> singleSpawnLocation = new List<Transform> { spawnLocator };

                // Purchase and instantiate the bed at the spawn location
                plantManager.PurchaseBed(unlockedBedPrefab, singleSpawnLocation, spawnScale, bedCost, true, false);

                // Optionally, add a new plant at the spawn location
                // Uncomment the next line if you want to add a plant
                plantManager.AddNewPlant(unlockedBedPrefab, spawnLocator, spawnScale, plantData);
            }

            isBedUnlocked = true;
            PlayerPrefs.SetInt("IsBedUnlocked", 1);
            PlayerPrefs.Save();

            Vector3 desiredScale = new Vector3(0.75f, 0.75f, 0.75f);
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
    
    public bool IsPlayerInRange(Transform target)
    {
        if (player == null) return false;
        return Vector3.Distance(player.position, target.position) <= interactionDistance;
    }
    
    public void ShowUnlockMenu()
    {
        if (unlockMenuUI != null)
            unlockMenuUI.SetActive(true);
    }
}
