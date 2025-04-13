
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlantManager : MonoBehaviour
{
    [System.Serializable]
    public class Plant
    {
        public GameObject basePrefab;
        public GameObject seedPrefab;
        public GameObject sproutPrefab;
        public GameObject maturePrefab;
        public InventoryData plantData;

        public GameObject baseStageObject;
        public GameObject currentStageObject;
        public int currentStage = -1;

        [HideInInspector] public int daysElapsed = 0;
        [HideInInspector] public bool isWatered = false;
        [HideInInspector] public bool isFertilized = false;
        [HideInInspector] public bool isReadytoHarvest;

        public Transform spawnLocator;
        public Vector3 spawnScale;
        public bool isBedLocked = true;
    }

    public int dayCounter = 1;
    public Text dayCounterText;

    private GameObject currentLockedInstance;
    public IntData playerCoins;

    public WateringInteraction wateringInteraction;
    public BackpackManager backpack;

    public List<Plant> plants = new List<Plant>();
    private List<GameObject> lockedBeds = new List<GameObject>();


    private void Start()
    {
        Application.targetFrameRate = 60;

        if (wateringInteraction == null)
        {
            wateringInteraction = FindObjectOfType<WateringInteraction>();
        }

        if (backpack == null)
        {
            backpack = FindObjectOfType<BackpackManager>();
        }

        UpdateDayCounterUI();
        //InitializeBeds();

        foreach (var plant in plants)
        {
            plant.currentStage = -1;

            if (plant.spawnLocator != null)
            {
                GameObject baseStage = Instantiate(
                    plant.basePrefab,
                    plant.spawnLocator.position,
                    plant.spawnLocator.rotation
                );

                baseStage.transform.localScale = plant.spawnScale;
                plant.currentStageObject = baseStage;
                plant.currentStage = -1;
            }
        }
    }

    private void InitializeBeds()
    {
        foreach (var plant in plants)
        {
            if (plant.isBedLocked)
            {
                // Instantiate locked bed prefab
                plant.baseStageObject = Instantiate(plant.basePrefab, plant.spawnLocator.position,
                    plant.spawnLocator.rotation);
                plant.baseStageObject.SetActive(false); // Hide locked bed
            }
            else
            {
                // Instantiate unlocked bed prefab
                plant.baseStageObject = Instantiate(plant.basePrefab, plant.spawnLocator.position,
                    plant.spawnLocator.rotation);
                plant.baseStageObject.SetActive(true);
            }
        }
    }

    private void EndDay()
    {
        List<Plant> plantsToRemove = new List<Plant>();

        foreach (var plant in plants)
        {
            if (plant.isWatered)
            {
                plant.daysElapsed++;
                int sproutDays = plant.isFertilized
                    ? Mathf.Max(1, plant.plantData.daysToSprout - 1)
                    : plant.plantData.daysToSprout;
                int matureDays = plant.isFertilized
                    ? Mathf.Max(1, plant.plantData.daysToMature - 1)
                    : plant.plantData.daysToMature;

                if (plant.currentStage == -1 && plant.daysElapsed >= sproutDays)
                {
                    SetStage(plant, 0); // Transition to seed stage
                }
                else if (plant.currentStage == 0 && plant.daysElapsed >= sproutDays)
                {
                    SetStage(plant, 1); // Transition to sprout stage
                }

                if (plant.currentStage == 1 && plant.daysElapsed >= sproutDays + matureDays)
                {
                    SetStage(plant, 2); // Transition to mature stage
                }
            }

            plant.isWatered = false;
            plant.isFertilized = false;
        }

        dayCounter++;

        foreach (var plant in plantsToRemove)
        {
            plants.Remove(plant);
        }

        ResetDirtToDry();
        UpdateDayCounterUI();
    }

    private void ResetDirtToDry()
    {
        if (wateringInteraction == null)
        {
            return;
        }

        foreach (var plant in plants)
        {
            if (plant.currentStageObject != null)
            {
                Transform[] childTransforms = plant.currentStageObject.GetComponentsInChildren<Transform>();

                foreach (Transform child in childTransforms)
                {
                    if (child.CompareTag("Dirt"))
                    {
                        MeshRenderer meshRenderer = child.GetComponent<MeshRenderer>();

                        if (meshRenderer != null)
                        {
                            meshRenderer.material = wateringInteraction.dirtDry;
                        }
                    }
                }
            }
        }
    }

    public void SetStage(Plant plant, int stage)
    {
        if (plant.currentStageObject != null)
        {
            Destroy(plant.currentStageObject); // Only destroy the current stage object
        }

        GameObject newStagePrefab = null;

        switch (stage)
        {
            case -1:
                newStagePrefab = plant.basePrefab; // Set to base prefab
                break;
            case 0:
                newStagePrefab = plant.seedPrefab;
                break;
            case 1:
                newStagePrefab = plant.sproutPrefab;
                break;
            case 2:
                newStagePrefab = plant.maturePrefab;
                plant.isReadytoHarvest = true;
                break;
        }

        if (newStagePrefab == null) return;

        plant.currentStageObject = Instantiate(newStagePrefab, plant.spawnLocator.position, Quaternion.identity);
        plant.currentStage = stage;

        if (stage == -1)
        {
            plant.daysElapsed = 0;
            plant.isWatered = false;
            plant.isFertilized = false;
            plant.isReadytoHarvest = false;
        }
    }

    private void UpdateDayCounterUI()
    {
        if (dayCounterText != null)
        {
            dayCounterText.text = "Day: " + dayCounter;
        }
    }

    private List<Plant> plantsToRemove = new List<Plant>();

    public void CollectPlant(PlantManager.Plant plant)
    {
        if (plant.currentStageObject == null)
        {
            return;
        }

        HarvestPlant harvestPlant = plant.currentStageObject.GetComponent<HarvestPlant>();
        if (harvestPlant == null || harvestPlant.GetProduceData() == null)
        {
            return;
        }

        ProduceData produceData = harvestPlant.GetProduceData();

        BackpackManager backpack = FindObjectOfType<BackpackManager>();
        if (backpack != null)
        {
            backpack.AddProduceToBackpack(produceData);
        }

        ResetPlantAfterCollection(plant);
    }

    private void ResetPlantAfterCollection(PlantManager.Plant plant)
    {
        if (plant.baseStageObject != null)
        {
            plant.baseStageObject.SetActive(true);
        }

        SetStage(plant, -1);
    }

    public void PlantSeedAt(Transform planterLocation)
    {
        InventoryData selectedSeed = backpack.GetSelectedSeed();

        Plant emptyBed = plants.Find(p => p.spawnLocator == planterLocation && p.currentStage == -1);

        if (selectedSeed.seedPrefab == null || selectedSeed.sproutPrefab == null || selectedSeed.maturePrefab == null)
        {
            return;
        }

        Plant newPlant = new Plant
        {
            plantData = selectedSeed,
            basePrefab = emptyBed.basePrefab,
            spawnLocator = planterLocation,
            spawnScale = emptyBed.spawnScale,
        };

        plants.Add(newPlant);
        backpack.RemoveSeed(selectedSeed);
    }

    public void AddNewPlant(GameObject basePrefab, Transform spawnLocator, Vector3 spawnScale, InventoryData plantData)
    {
        // Instantiate the base stage object
        GameObject baseStage = Instantiate(basePrefab, spawnLocator.position, spawnLocator.rotation);
        baseStage.transform.localScale = spawnScale;

        // Create a new Plant instance
        Plant newPlant = new Plant
        {
            basePrefab = basePrefab,
            spawnLocator = spawnLocator,
            spawnScale = spawnScale,
            plantData = plantData,
            baseStageObject = baseStage,
            currentStageObject = baseStage,
            currentStage = -1
        };

        // Add the new plant to the list
        plants.Add(newPlant);
    }

    public void PurchaseBed(GameObject bedPrefab, List<Transform> spawnLocations, Vector3 spawnScale, int cost,
        bool isUnlocking, bool instantiateBed = true)
    {
        if (playerCoins.value >= cost)
        {
            playerCoins.value -= cost;

            if (isUnlocking)
            {
                // Deactivate all GameObjects tagged as "LockedBed"
                GameObject[] lockedBeds = GameObject.FindGameObjectsWithTag("LockedBed");
                foreach (GameObject bed in lockedBeds)
                {
                    bed.SetActive(false);
                }

                if (instantiateBed)
                {
                    // Instantiate unlocked beds at specified locations
                    foreach (Transform spawnLocation in spawnLocations)
                    {
                        GameObject newBed = Instantiate(bedPrefab, spawnLocation.position, spawnLocation.rotation);
                        newBed.transform.localScale = spawnScale;
                    }
                }

                Debug.Log("Unlocked beds purchased and instantiated.");
            }
            else
            {
                if (spawnLocations.Count > 0 && instantiateBed)
                {
                    Transform spawnLocator = spawnLocations[0];
                    GameObject lockedBed = Instantiate(bedPrefab, spawnLocator.position, spawnLocator.rotation);
                    lockedBed.transform.localScale = spawnScale;
                    lockedBed.tag = "LockedBed";
                    lockedBed.SetActive(false);

                    Debug.Log("Locked bed purchased and instantiated.");
                }
                else
                {
                    Debug.LogWarning("No spawn locations provided for locked bed or instantiation disabled.");
                }
            }
        }
        else
        {
            Debug.Log("Not enough coins to purchase the bed.");
        }
    }
}
