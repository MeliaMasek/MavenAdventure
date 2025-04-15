using UnityEngine;
//Code created, borrowed, and modified from Chat GPT

public class SeedManager : MonoBehaviour
{
    public bool isPlantingMode = false;
    public LayerMask plantLayer;
    public PlantManager plantManager;
    public Material dirtDry;
    
    public AudioClip plantingSound;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void ActivatePlantingMode()
    {
        FindObjectOfType<WateringInteraction>().DeactivateWateringMode();
        isPlantingMode = true; // ✅ Enable planting mode when selecting a seed
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (isPlantingMode && Physics.Raycast(ray, out hit, 100f, plantLayer.value))
            {
                foreach (var plant in plantManager.plants)
                {
                    if (plant.currentStageObject == hit.collider.gameObject)  
                    {
                        if (plant.currentStage == -1 && plant.currentStageObject.CompareTag("PlantBed"))  
                        {
                            PlantBed plantBed = hit.collider.GetComponentInParent<PlantBed>();
                            if (plantBed != null)
                            {
                                PlantSeed(plant, plantBed);
                            }
                        }
                        isPlantingMode = false;
                        return;
                    }
                }
            }

            if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("Harvest")))
            {
                PlantManager.Plant plant = plantManager.plants.Find(p => p.currentStageObject == hit.collider.gameObject);
                if (plant != null)
                {
                    plantManager.CollectPlant(plant);
                }
            }
        }
    }

    public void PlantSeed(PlantManager.Plant oldPlant, PlantBed plantBed)
    {
        if (oldPlant.currentStage != -1)
        {
            return;
        }

        // Get the selected seed from the backpack
        InventoryData selectedSeed = FindObjectOfType<BackpackManager>().GetSelectedSeed();
        if (selectedSeed == null) return;
        if (selectedSeed.seedPrefab == null || selectedSeed.sproutPrefab == null || selectedSeed.maturePrefab == null) return;

        oldPlant.baseStageObject = oldPlant.currentStageObject;

        if (oldPlant.baseStageObject != null && oldPlant.baseStageObject.CompareTag("PlantBed"))
        {
            oldPlant.baseStageObject.SetActive(false);
        }

        PlantManager.Plant newPlant = new PlantManager.Plant
        {
            plantData = selectedSeed,
            seedPrefab = selectedSeed.seedPrefab,
            sproutPrefab = selectedSeed.sproutPrefab,
            maturePrefab = selectedSeed.maturePrefab,
            spawnLocator = oldPlant.spawnLocator,
            spawnScale = Vector3.one,
            baseStageObject = oldPlant.baseStageObject
        };

        newPlant.currentStageObject = Instantiate(newPlant.seedPrefab, newPlant.spawnLocator.position, Quaternion.identity);
        
        if (plantingSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(plantingSound);
        }
        
        newPlant.currentStage = 0;

        plantManager.plants.Add(newPlant);
    
        // Call the calendar manager to mark the harvest day
        // Assuming growth duration is stored in PlantData and is in days
        int growthDuration = selectedSeed.daysToMature;  // Ensure you have growth duration in your seed data
        Sprite plantIcon = selectedSeed.produceData.produceIcon;  // Accessing the icon from ProduceData

        // Call the CalendarManager to register the harvest day and icon
        CalendarManager calendarManager = FindObjectOfType<CalendarManager>();
        calendarManager.MarkHarvestDay(calendarManager.GetCurrentDay(), growthDuration, plantIcon, selectedSeed);

        // Remove the seed from the backpack
        FindObjectOfType<BackpackManager>().RemoveItem(selectedSeed);
    }

    
    private void ChangeDirtMaterial(GameObject stageObject)
    {
        Transform[] childTransforms = stageObject.GetComponentsInChildren<Transform>();

        bool materialChanged = false;

        foreach (Transform child in childTransforms)
        {
            if (child.CompareTag("Dirt"))
            {
                MeshRenderer meshRenderer = child.GetComponent<MeshRenderer>();

                if (meshRenderer != null)
                {
                    meshRenderer.material = dirtDry;
                    materialChanged = true;
                }
            }
        }
    }
}
