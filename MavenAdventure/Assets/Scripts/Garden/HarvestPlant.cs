using UnityEngine;

public class HarvestPlant : MonoBehaviour
{
    private PlantManager plantManager;
    private PlantManager.Plant plantData; // Assuming Plant is a class, not a struct
    public ProduceData produceData; // Reference to the produce data for this plant

    private void Start()
    {
        plantManager = FindObjectOfType<PlantManager>();

        if (plantData == null)
        {
            Debug.LogWarning($"Plant data not assigned for {gameObject.name}. Call SetPlantData().");
        }
    }

    private void OnMouseDown()
    {
        if (plantData != null && plantData.isReadytoHarvest)
        {
            // Assuming you have a HarvestManager instance
            HarvestManager harvestManager = FindObjectOfType<HarvestManager>();
            if (harvestManager != null)
            {
                harvestManager.HarvestPlant(plantData, produceData); // Pass the produce data as well
            }
            else
            {
                Debug.LogError("No HarvestManager found in the scene.");
            }
        }
    }

    public void SetPlantData(PlantManager.Plant plant)
    {
        plantData = plant;
    }
}