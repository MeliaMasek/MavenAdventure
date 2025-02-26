using UnityEngine;

public class PlantInteraction : MonoBehaviour
{
    public PlantManager.Plant plantData; // Reference to the plant's data
    public BackpackManager backpackManager; // Reference to the BackpackManager for seed selection

    private void Start()
    {
        // Ensure the BackpackManager is assigned
        if (backpackManager == null)
        {
            backpackManager = FindObjectOfType<BackpackManager>();
            if (backpackManager == null)
            {
                Debug.LogError("No BackpackManager found in the scene.");
            }
        }
    }

    private void OnMouseDown()
    {
        Debug.Log("OnMouseDown triggered."); // Add this line to check if the method is called
        if (plantData == null || plantData.currentStageObject == null)
        {
            Debug.LogWarning("No plant data or current stage object assigned.");
            return;
        }

        // Assuming this is how you get the produce data for the plant
        ProduceData produceData = plantData.produceData;

        Debug.Log($"Clicked on plant: {plantData.currentStageObject.name}");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == null) return;

        // Check if the collider is the watering can
        if (other.CompareTag("WateringCan"))
        {
            Debug.Log($"Water can is over the plant: {plantData.currentStageObject?.name ?? "Unnamed Plant"}");

            // Trigger watering logic
            if (plantData != null)
            {
                plantData.isWatered = true;
                Debug.Log($"Plant {plantData.currentStageObject?.name ?? "Unnamed Plant"} is now watered.");
            }
        }
        else if (other.CompareTag("PlantBed"))
        {
            // Check if the player has a seed selected
            if (backpackManager.GetSelectedSeed() == null)
            {
                Debug.Log("No seed selected to plant.");
                return;
            }

            // Plant the selected seed
            PlantSelectedSeed(other.transform);
        }
    }

    private void PlantSelectedSeed(Transform planterLocation)
    {
        InventoryData selectedSeed = backpackManager.GetSelectedSeed();

        // Create a new plant instance using the selected seed
        PlantManager.Plant newPlant = new PlantManager.Plant
        {
            plantData = selectedSeed,
            basePrefab = selectedSeed.seedPrefab, // Use the seed prefab
            spawnLocator = planterLocation,
            spawnScale = Vector3.one
        };

        // Add this new plant to the PlantManager
        FindObjectOfType<PlantManager>().plants.Add(newPlant);
        FindObjectOfType<PlantManager>().SetStage(newPlant, 0); // Plant the seed

        // Remove the seed from the backpack
        backpackManager.RemoveItem(selectedSeed);
        Debug.Log($"Planted {selectedSeed.displayName} at {planterLocation.position}");

        // Optionally close the inventory UI
        backpackManager.ToggleBackpack(false);
    }
}
