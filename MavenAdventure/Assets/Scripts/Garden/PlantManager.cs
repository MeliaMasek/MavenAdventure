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

private void EndDay()
{
    List<Plant> plantsToRemove = new List<Plant>();

    foreach (var plant in plants)
    {
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
}
