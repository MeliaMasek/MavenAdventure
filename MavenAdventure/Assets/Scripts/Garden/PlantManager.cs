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
        public ProduceData produceData;

        [HideInInspector] public GameObject currentStageObject;
        [HideInInspector] public int currentStage = -1;
        [HideInInspector] public int daysElapsed = 0;
        [HideInInspector] public bool isWatered = false;
        [HideInInspector] public bool isFertilized = false;

        public Transform spawnLocator;
        public Vector3 spawnScale;
        public bool isReadytoHarvest;
    }

    public int dayCounter = 1;
    public Text dayCounterText;

    public WateringInteraction wateringInteraction;
    public BackpackManager backpack;
    
    public List<Plant> plants = new List<Plant>();

    private void Start()
    {
        if (wateringInteraction == null)
        {
            wateringInteraction = FindObjectOfType<WateringInteraction>();
        }

        if (backpack == null)
        {
            backpack = FindObjectOfType<BackpackManager>();
        }

        UpdateDayCounterUI();

        foreach (var plant in plants)
        {
            plant.currentStage = -1; // Ensure all start empty
            //Debug.Log($"Initialized bed at {plant.spawnLocator.position} with stage {plant.currentStage}");
            
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

private void EndDay()
{
    List<Plant> plantsToRemove = new List<Plant>();

    foreach (var plant in plants)
    {
        //Debug.Log($"Plant: {plant.plantData.displayName}, Days Elapsed: {plant.daysElapsed}, Current Stage: {plant.currentStage}");

        if (plant.isWatered)
        {
            plant.daysElapsed++;
            int sproutDays = plant.isFertilized ? Mathf.Max(1, plant.plantData.daysToSprout - 1) : plant.plantData.daysToSprout;
            int matureDays = plant.isFertilized ? Mathf.Max(1, plant.plantData.daysToMature - 1) : plant.plantData.daysToMature;

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
        Debug.Log("started set stage");
        if (plant.currentStageObject != null)
        {
            Debug.Log("object plant is null, destroying object");
            Destroy(plant.currentStageObject);
        }
        Debug.Log("object plant is not null");
        GameObject newStagePrefab = null;
        
        switch (stage)
        {
            case -1:  // Empty bed stage
                Debug.Log("case -1");
                newStagePrefab = plant.basePrefab;  // Ensure basePrefab is correctly set to the empty bed
                break;
            case 0:
                Debug.Log("case 0");
                newStagePrefab = plant.seedPrefab;
                break;
            case 1:
                Debug.Log("case 1");
                newStagePrefab = plant.sproutPrefab;
                break;
            case 2:
                Debug.Log("case 2");
                newStagePrefab = plant.maturePrefab;
                break;
        }

        if (newStagePrefab == null)
        {
            return;
        }
        Debug.Log("completed type check. intsantied new stage prefab");
        plant.currentStageObject = Instantiate(newStagePrefab, plant.spawnLocator.position, Quaternion.identity);
        plant.currentStage = stage;  // Update plant's growth stage

        // If resetting to an empty bed, clear any plant data
        if (stage == -1)
        {
            Debug.Log("reset bed");
            plant.daysElapsed = 0;
            plant.isWatered = false;
            plant.isFertilized = false;
        }
        Debug.Log("completed set stage");
    }
    
    private void UpdateDayCounterUI()
    {
        if (dayCounterText != null)
        {
            dayCounterText.text = "Day: " + dayCounter;
        }
    }

    private List<Plant> plantsToRemove = new List<Plant>();

    public void CollectPlant(Plant plant)
    {
        if (plant.isReadytoHarvest)
        {
            // Reference to the corresponding ProduceData
            //ProduceData produceData = plant.produceData;  // Use plant.produceData instead of plantData.produceData

            // Add the harvested produce to the backpack
            //backpack.AddToBackpack(produceData);

            // Reset plant state to empty bed
            SetStage(plant, -1);  // Reset to empty bed
            plant.daysElapsed = 0;  // Reset growth timer
            plant.isWatered = false;
            plant.isFertilized = false;
            plant.currentStage = -1;

            // Mark the planter location as available for new planting
            if (plant.spawnLocator.TryGetComponent(out EmptyBed emptyBed))
            {
                emptyBed.isOccupied = false;
            }

            Debug.Log($"{plant} collected! The bed is now empty.");
        }
        else
        {
            Debug.Log($"{plant.plantData.displayName} is not ready for harvest.");
        }
    }
    
    public void PlantSeedAt(Transform planterLocation)
    {
        Debug.Log("PlantSeedAt method called!");
    
        InventoryData selectedSeed = backpack.GetSelectedSeed(); // Get the selected seed from BackpackManager

        if (selectedSeed == null)
        {
            Debug.LogError("Planting failed: No seed selected from BackpackManager!");
            return;
        }
    
        Debug.Log($"Planting Seed: {selectedSeed.displayName} at {planterLocation.position}");

        Plant emptyBed = plants.Find(p => p.spawnLocator == planterLocation && p.currentStage == -1);
        if (emptyBed == null)
        {
            Debug.LogError("No empty bed found at the location!");
            return;
        }

        // Check if the selected seed has the necessary prefabs (seed, sprout, mature)
        if (selectedSeed.seedPrefab == null || selectedSeed.sproutPrefab == null || selectedSeed.maturePrefab == null)
        {
            Debug.LogError("Selected seed is missing one of the required prefabs!");
            return;
        }

        // Create a new plant instance and assign the correct seed data
        Plant newPlant = new Plant
        {
            plantData = selectedSeed,
            produceData = selectedSeed.produceData,
            basePrefab = emptyBed.basePrefab,
            spawnLocator = planterLocation,
            spawnScale = emptyBed.spawnScale,
        };

        plants.Add(newPlant);
        backpack.RemoveSeed(selectedSeed); // Remove seed from backpack
        Debug.Log($"Planted {selectedSeed.displayName} at {planterLocation.position}");
    }
}
