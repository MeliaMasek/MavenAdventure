using UnityEngine;
using UnityEngine.UI; // Import UI namespace

public class WateringInteraction : MonoBehaviour
{
    public bool isWateringMode = false; // Tracks if watering mode is active
    public LayerMask plantLayer;       // Layer for plant objects
    public PlantManager plantManager;  // Reference to the PlantManager script
    public BackpackManager backpackManager; // Reference to BackpackManager
    public Material dirtWet;           // Material for wet dirt
    public Material dirtDry;           // Material for dry dirt

    public TimedEnergyBar energyBar;  // Reference to Energy Bar
    public Button wateringButton;     // Reference to the UI Button

    private void Start()
    {
        // Ensure references are assigned
        if (plantManager == null)
        {
            plantManager = FindObjectOfType<PlantManager>();
        }

        if (backpackManager == null)
        {
            backpackManager = FindObjectOfType<BackpackManager>();
        }
    }

    public void ActivateWateringMode()
    {
        if (!isWateringMode) // Prevent re-activating and consuming energy
        {
            isWateringMode = true;
            wateringButton.interactable = false; // Disable button while active
            Debug.Log("Watering mode activated. Click a plant to water.");
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 100f, plantLayer))
            {
                foreach (var plant in plantManager.plants)
                {
                    if (plant.currentStageObject == hit.collider.gameObject)
                    {
                        if (plant.currentStage == 2) // Fully grown stage
                        {
                            CollectPlant(plant); // Use new collection function
                            return;
                        }
                        else if (isWateringMode) // Only water if in watering mode
                        {
                            bool watered = TryWaterPlant(hit.collider.gameObject);

                            if (watered) 
                            {
                                isWateringMode = false;
                                energyBar.ReduceEnergyOnClick();
                                wateringButton.interactable = true;
                            }
                        }
                    }
                }
            }
        }
    }

    private void CollectPlant(PlantManager.Plant plant)
    {
        if (plant.currentStage == 2) // Fully grown
        {
            if (backpackManager != null)
            {
                backpackManager.AddToBackpack(plant.plantData); // Add to backpack UI
                Debug.Log($"Collected {plant.maturePrefab.name} and added to backpack.");
            }
            else
            {
                Debug.LogError("BackpackManager reference is missing!");
            }

            Destroy(plant.currentStageObject); // Remove from scene
            plantManager.plants.Remove(plant); // Remove from list
        }
    }

    private bool TryWaterPlant(GameObject clickedObject)
    {
        foreach (var plant in plantManager.plants)
        {
            if (plant.currentStageObject == clickedObject)
            {
                if (plant.currentStage == -1)
                {
                    Debug.Log("Cannot water base stage. Plant a seed instead.");
                    return false; // Watering failed
                }

                if (!plant.isWatered)
                {
                    plant.isWatered = true;
                    ChangeDirtMaterial(plant.currentStageObject);
                    Debug.Log($"Watered the plant: {clickedObject.name}");
                    return true; // Successfully watered
                }
                else
                {
                    Debug.Log("This plant is already watered today.");
                    return false; // Already watered
                }
            }
        }
        return false; // No matching plant found
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
                    meshRenderer.material = dirtWet; // Change to wet material
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

    public bool HasUnwateredPlants()
    {
        foreach (var plant in plantManager.plants)
        {
            if (!plant.isWatered && plant.currentStage != -1)
            {
                return true; // At least one plant needs water
            }
        }
        return false;
    }
}
