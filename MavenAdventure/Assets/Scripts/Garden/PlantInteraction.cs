using UnityEngine;

public class PlantInteraction : MonoBehaviour
{
    public PlantManager.Plant plantData; // Reference to the plant's data

    private void OnMouseDown()
    {
        if (plantData == null || plantData.currentStageObject == null)
        {
            Debug.LogWarning("No plant data or current stage object assigned to this PlantInteraction script.");
            return;
        }

        // Log the plant interaction
        Debug.Log($"Clicked on plant: {plantData.currentStageObject.name}");

        // Find the PlantInfo UI to display details about this plant
        PlantInfo plantInfoUI = FindObjectOfType<PlantInfo>();
        if (plantInfoUI != null)
        {
            plantInfoUI.ShowPlantInfo(plantData);
        }
        else
        {
            Debug.LogWarning("PlantInfo UI is not found in the scene.");
        }
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
    }
}