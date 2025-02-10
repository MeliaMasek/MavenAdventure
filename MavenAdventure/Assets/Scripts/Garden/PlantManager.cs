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
        public InventoryData plantData; // Reference to the scriptable object

        public int daysToSprout = 2;
        public int daysToMature = 5;

        [HideInInspector] public GameObject currentStageObject;
        [HideInInspector] public int currentStage = -1;
        [HideInInspector] public int daysElapsed = 0;
        [HideInInspector] public bool isWatered = false;
        [HideInInspector] public bool isFertilized = false;

        public Transform spawnLocator; // Reference to the locator's Transform
        public Vector3 spawnScale;    // Store the initial scale
    }

    public int dayCounter = 1; 
    public Text dayCounterText;

    public WateringInteraction wateringInteraction; // Reference to the watering script
    public BackpackManager backpack;  // Reference to the backpack system

    public List<Plant> plants = new List<Plant>();

    private void Start()
    {
        if (wateringInteraction == null)
        {
            wateringInteraction = FindObjectOfType<WateringInteraction>(); // Auto-assign if not set
        }

        if (backpack == null)
        {
            backpack = FindObjectOfType<BackpackManager>(); // Auto-assign if not set
        }
        
        UpdateDayCounterUI(); // Initialize UI
        
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
            else
            {
                Debug.LogWarning("Spawn locator not assigned for a plant!");
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
                int sproutDays = plant.isFertilized ? Mathf.Max(1, plant.daysToSprout - 1) : plant.daysToSprout;
                int matureDays = plant.isFertilized ? Mathf.Max(1, plant.daysToMature - 1) : plant.daysToMature;

                if (plant.currentStage == -1 && plant.daysElapsed >= sproutDays)
                {
                    SetStage(plant, 0);
                }
                else if (plant.currentStage == 0 && plant.daysElapsed >= sproutDays)
                {
                    SetStage(plant, 1);
                }
                else if (plant.currentStage == 1 && plant.daysElapsed >= matureDays)
                {
                    SetStage(plant, 2);
                }
            }

            plant.isWatered = false;
            plant.isFertilized = false;
        }

        dayCounter++;

        // Remove collected plants safely after iteration
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
            Debug.LogError("WateringInteraction reference is missing!");
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
                            meshRenderer.material = wateringInteraction.dirtDry; // Set dirt back to dry
                            Debug.Log($"Dirt reset to dry for {child.name} in {plant.currentStageObject.name}.");
                        }
                    }
                }
            }
        }
    }

    private void SetStage(Plant plant, int stage)
    {
        if (plant.currentStageObject != null)
        {
            Destroy(plant.currentStageObject);
        }

        GameObject newStagePrefab = null;

        switch (stage)
        {
            case -1:
                newStagePrefab = plant.basePrefab;
                break;
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

        if (newStagePrefab != null)
        {
            plant.currentStageObject = Instantiate(
                newStagePrefab,
                plant.spawnLocator.position,
                plant.spawnLocator.rotation
            );

            plant.currentStageObject.transform.localScale = plant.spawnScale;
        }

        plant.currentStage = stage;

        if (stage < 2)
        {
            plant.daysElapsed = 0;
        }

        Debug.Log($"Set plant to stage {stage} at position {plant.spawnLocator.position}");
    }

    private void UpdateDayCounterUI()
    {
        if (dayCounterText != null)
        {
            dayCounterText.text = "Day: " + dayCounter; // Update UI text
        }
    }

    private List<Plant> plantsToRemove = new List<Plant>();

    public void CollectPlant(Plant plant)
    {
        if (plant.currentStage == 2) // Fully grown
        {
            FindObjectOfType<BackpackManager>().AddToBackpack(plant.plantData); // Add to backpack
            Destroy(plant.currentStageObject); // Remove the mature plant model

            // Reset plant back to empty stage
            SetStage(plant, -1);
            plant.daysElapsed = 0;
            plant.isWatered = false;
            plant.isFertilized = false;

            Debug.Log($"Plant collected and planter reverted to empty stage.");
        }
    }
    
    public void PlantSeedAt(Transform planterLocation)
    {
        if (backpack.GetSelectedSeed() == null)
        {
            Debug.Log("No seed selected!");
            return;
        }

        InventoryData selectedSeed = backpack.GetSelectedSeed();

        // Create a new Plant instance using the selected seed
        Plant newPlant = new Plant
        {
            plantData = selectedSeed,
            basePrefab = selectedSeed.seedPrefab, // Uses the correct prefab
            spawnLocator = planterLocation,
            spawnScale = Vector3.one
        };

        plants.Add(newPlant);
        SetStage(newPlant, 0); // Plant at seed stage
        Debug.Log($"Planted {selectedSeed.displayName} at {planterLocation.position}");

        // Reduce the seed count in the backpack
        backpack.RemoveItem(selectedSeed);
    }
}
