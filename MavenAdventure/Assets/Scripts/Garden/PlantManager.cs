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
        
        [HideInInspector] public GameObject currentStageObject;
        [HideInInspector] public int currentStage = -1;
        [HideInInspector] public int daysElapsed = 0;
        [HideInInspector] public bool isWatered = false;
        [HideInInspector] public bool isFertilized = false;

        public Transform spawnLocator;
        public Vector3 spawnScale;
        public bool isHarvestable => currentStage == 2;
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
        Debug.Log($"Plant: {plant.plantData.displayName}, Days Elapsed: {plant.daysElapsed}, Current Stage: {plant.currentStage}");

        if (plant.isWatered)
        {
            plant.daysElapsed++;
            int sproutDays = plant.isFertilized ? Mathf.Max(1, plant.plantData.daysToSprout - 1) : plant.plantData.daysToSprout;
            int matureDays = plant.isFertilized ? Mathf.Max(1, plant.plantData.daysToMature - 1) : plant.plantData.daysToMature;

            Debug.Log($"Sprout Days: {sproutDays}, Mature Days: {matureDays}, Days Elapsed: {plant.daysElapsed}");

            // Transition from empty bed to seed (stage -1 to 0)
            if (plant.currentStage == -1 && plant.daysElapsed >= sproutDays)
            {
                Debug.Log($"Transitioning {plant.plantData.displayName} from Empty Bed to Seed (Stage 0)");
                SetStage(plant, 0); // Transition to seed stage
            }
            // Transition from seed to sprout (stage 0 to 1)
            else if (plant.currentStage == 0 && plant.daysElapsed >= sproutDays)
            {
                Debug.Log($"Transitioning {plant.plantData.displayName} from Seed to Sprout (Stage 1)");
                SetStage(plant, 1); // Transition to sprout stage
            }
            // Transition from sprout to mature (stage 1 to 2), only if plant is already sprout
            if (plant.currentStage == 1 && plant.daysElapsed >= sproutDays + matureDays)
            {
                Debug.Log($"Transitioning {plant.plantData.displayName} from Sprout to Mature (Stage 2)");
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
            Destroy(plant.currentStageObject);
        }

        GameObject newStagePrefab = null;

        switch (stage)
        {
            case 0:
                newStagePrefab = plant.seedPrefab;
                break;
            case 1:
                newStagePrefab = plant.sproutPrefab;
                break;
            case 2:
                newStagePrefab = plant.maturePrefab;
                break;
        }

        if (newStagePrefab == null)
        {
            return;
        }

        plant.currentStageObject = Instantiate(newStagePrefab, plant.spawnLocator.position, Quaternion.identity);
        plant.currentStage = stage;  // Update plant's growth stage

        // Mark as harvestable if it's mature
        if (plant.currentStage == 2)
        {
            Debug.Log($"{plant.plantData.displayName} is ready to be harvested.");
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

    public void CollectPlant(Plant plant)
    {
        if (plant.isHarvestable)
        {
            // Add the mature plant to the backpack
            backpack.AddToBackpack(plant.plantData);

            // Reset the plant to an empty bed
            SetStage(plant, -1); // Set back to empty stage
            plant.spawnLocator.GetComponent<EmptyBed>().isOccupied = false;

            Debug.Log($"{plant.plantData.displayName} collected and added to backpack!");
        }
        else
        {
            Debug.Log($"{plant.plantData.displayName} is not ready for harvest.");
        }
    }
    public void PlantSeedAt(Transform planterLocation)
    {
        Plant emptyBed = plants.Find(p => p.spawnLocator == planterLocation && p.currentStage == -1);

        if (emptyBed == null)
        {
            return;
        }

        InventoryData selectedSeed = backpack.GetSelectedSeed();
        if (selectedSeed == null)
        {
            return;
        }

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

        plants.Remove(emptyBed);
        plants.Add(newPlant);

        SetStage(newPlant, 0); // Start at seed stage

        backpack.RemoveItem(selectedSeed);
    }
}
