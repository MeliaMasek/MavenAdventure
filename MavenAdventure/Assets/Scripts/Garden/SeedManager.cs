using UnityEngine;

public class SeedManager : MonoBehaviour
{
    public bool isPlantingMode = false; // Tracks if planting mode is active
    public LayerMask plantLayer;       // Layer for plant objects
    public PlantManager plantManager;  // Reference to the PlantManager script
    public Material dirtDry;           // Material for dry dirt

    // Called when the Planting Button is clicked
    public void ActivatePlantingMode()
    {
        isPlantingMode = true;
        Debug.Log("Planting mode activated. Click a base stage plant to plant a seed.");
    }

    private void Update()
    {
        if (isPlantingMode && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 100f, plantLayer.value))
            {
                Debug.Log($"Hit object: {hit.collider.gameObject.name}");

                foreach (var plant in plantManager.plants)
                {
                    if (plant.currentStageObject == hit.collider.gameObject)  
                    {
                        if (plant.currentStage == -1)
                        {
                            PlantBed plantBed = hit.collider.GetComponentInParent<PlantBed>();
                            if (plantBed != null)
                            {
                                PlantSeed(plant, plantBed);  // ✅ Now passes both required parameters
                            }
                            else
                            {
                                Debug.LogError("No PlantBed found for this plant!");
                            }
                        }
                        else
                        {
                            Debug.Log("This plant is not in the base stage.");
                        }

                        isPlantingMode = false;
                        break;
                    }
                }
            }
            else
            {
                Debug.Log("Raycast did not hit anything.");
            }
        }
    }

    public void PlantSeed(PlantManager.Plant oldPlant, PlantBed plantBed)
    {
        InventoryData selectedSeed = FindObjectOfType<BackpackManager>().GetSelectedSeed();
        if (selectedSeed == null)
        {
            Debug.LogError("No seed selected! Select a seed before planting.");
            return;
        }

        if (selectedSeed.seedPrefab == null)
        {
            Debug.LogError($"❌ ERROR: No seed prefab set for {selectedSeed.displayName}! Check InventoryData.");
            return;
        }

        if (selectedSeed.sproutPrefab == null || selectedSeed.maturePrefab == null)
        {
            Debug.LogError($"❌ ERROR: Missing growth stage prefabs for {selectedSeed.displayName}! Check InventoryData.");
            return;
        }

        Debug.Log($"🌱 Planting seed: {selectedSeed.displayName}");

        // Destroy the old plant object (previous base bed)
        Destroy(oldPlant.currentStageObject);

        // ✅ Assign all prefabs properly
        PlantManager.Plant newPlant = new PlantManager.Plant
        {
            plantData = selectedSeed,
            seedPrefab = selectedSeed.seedPrefab,      // ✅ Store Seed Prefab
            sproutPrefab = selectedSeed.sproutPrefab,  // ✅ Store Sprout Prefab
            maturePrefab = selectedSeed.maturePrefab,  // ✅ Store Mature Prefab
            spawnLocator = oldPlant.spawnLocator,      // Keep the same location
            spawnScale = Vector3.one
        };

        // Instantiate the correct seed prefab
        newPlant.currentStageObject = Instantiate(newPlant.seedPrefab, newPlant.spawnLocator.position, Quaternion.identity);
        newPlant.currentStage = 0;

        // Remove the old plant from the list and add the new one
        plantManager.plants.Remove(oldPlant);
        plantManager.plants.Add(newPlant);

        Debug.Log($"🌱 Seed planted: {selectedSeed.displayName} at {newPlant.spawnLocator.position}");

        // Remove 1 seed from inventory
        FindObjectOfType<BackpackManager>().RemoveItem(selectedSeed);
    }

    
    private void ChangeDirtMaterial(GameObject stageObject)
    {
        // Find all child objects with the "Dirt" tag
        Transform[] childTransforms = stageObject.GetComponentsInChildren<Transform>();

        bool materialChanged = false;

        foreach (Transform child in childTransforms)
        {
            if (child.CompareTag("Dirt"))
            {
                MeshRenderer meshRenderer = child.GetComponent<MeshRenderer>();

                if (meshRenderer != null)
                {
                    meshRenderer.material = dirtDry; // Change to wet material
                    materialChanged = true;
                    Debug.Log($"Changed material to wet for {child.name} in {stageObject.name}.");
                }
                else
                {
                    Debug.LogWarning($"No MeshRenderer found on {child.name} tagged as 'Dirt'.");
                }
            }
        }

        if (!materialChanged)
        {
            Debug.LogWarning($"No objects tagged as 'Dirt' found in {stageObject.name}.");
        }
    }
}