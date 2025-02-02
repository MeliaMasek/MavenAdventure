using System.Collections.Generic;
using UnityEngine;

public class PlayerBackpack : MonoBehaviour
{
    public List<PlantManager.Plant> collectedPlants = new List<PlantManager.Plant>();
    public BackpackManager backpackManager; // Reference to the BackpackManager for updating UI

    // Add a plant to the backpack
    public void AddToBackpack(PlantManager.Plant plant)
    {
        collectedPlants.Add(plant);
        Debug.Log($"Added {plant.currentStageObject.name} to backpack. Total items: {collectedPlants.Count}");

        // Update the backpack UI
        if (backpackManager != null)
        {
            backpackManager.UpdateBackpackUI();
        }
    }
}