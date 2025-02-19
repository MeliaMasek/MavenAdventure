using UnityEngine;

public class HarvestManager : MonoBehaviour
{
    private BackpackManager backpackManager;
    private PlantManager plantManager;
    private void Start()
    {
        backpackManager = FindObjectOfType<BackpackManager>();
    }

    public void HarvestPlant(PlantManager.Plant plant, ProduceData produceData)
    {
        if (produceData == null)
        {
            Debug.LogError("Produce data is null!");
            return;
        }

        // Add the produce to the backpack
        backpackManager.AddProduceToBackpack(produceData); // Assuming this method exists
        Debug.Log($"Harvested {produceData.displayName} from {plant.currentStageObject.name}");

        // Reset the plant stage
        plantManager.SetStage(plant, -1); // Reset to empty stage or initial stage
    }
}