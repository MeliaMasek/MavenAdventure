using UnityEngine;

public class HarvestPlant : MonoBehaviour
{
    private PlantManager plantManager;
    private PlantManager.Plant plantData; // Assuming Plant is a class, not a struct
    public ProduceData produceData; // Reference to the produce data for this plant

    private void Start()
    {
        plantManager = FindObjectOfType<PlantManager>();
    }

    private void OnMouseDown()
    {
        if (plantData == null)
        {
            return; // Prevent interaction until data is set
        }

        if (plantData.isReadytoHarvest)
        {
            HarvestManager harvestManager = FindObjectOfType<HarvestManager>();
            if (harvestManager != null)
            {
                harvestManager.HarvestPlant(plantData, produceData);
            }
        }
    }

    public void SetPlantData(PlantManager.Plant plant)
    {
        plantData = plant;
    }
}