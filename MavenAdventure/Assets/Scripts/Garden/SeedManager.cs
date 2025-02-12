
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

            if (Physics.Raycast(ray, out RaycastHit hit, 100f, plantLayer))
            {
                Debug.Log($"Hit object: {hit.collider.gameObject.name}");

                // Find the plant associated with the clicked GameObject
                foreach (var plant in plantManager.plants)
                {
                    if (plant.currentStageObject == hit.collider.gameObject)  
                    {
                        // Only allow planting if the plant is still in the base stage (-1)
                        if (plant.currentStage == -1)
                        {
                            PlantSeed(plant);
                        }
                        else
                        {
                            Debug.Log("This plant is not in the base stage and cannot be planted.");
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

    private void PlantSeed(PlantManager.Plant plant)
    {
        Debug.Log($"Planting seed at {plant.currentStageObject.transform.position}");

        // Change dirt material to indicate planting
        ChangeDirtMaterial(plant.currentStageObject);

        // Destroy the base stage object
        Destroy(plant.currentStageObject);

        // Instantiate the seed stage at the same position
        plant.currentStageObject = Instantiate(plant.seedPrefab, plant.spawnLocator.position, Quaternion.identity);

        // Update plant's current stage
        plant.currentStage = 0;

        Debug.Log($"Seed planted at {plant.spawnLocator.position}");
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
