using UnityEngine;

public class PlantInteraction : MonoBehaviour
{
    public PlantManager.Plant plantData; // Reference to the plant's data

    private void OnMouseDown()
    {
        // Check if the player clicked this plant
        Debug.Log($"Clicked on plant: {plantData.baseStage.name}");

        // Access the PlantInfoUI to show details about this plant
        PlantInfo plantInfoUI = FindObjectOfType<PlantInfo>();
        if (plantInfoUI != null)
        {
            plantInfoUI.ShowPlantInfo(plantData);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        // Detects if the water can icon enters the plant area
        if (other.CompareTag("WateringCan"))
        {
            Debug.Log("Water can is over this plant");
        }
    }
}