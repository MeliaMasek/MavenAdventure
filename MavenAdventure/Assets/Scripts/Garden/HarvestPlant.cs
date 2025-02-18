using UnityEngine;

public class HarvestPlant : MonoBehaviour
{
    private PlantManager plantManager;
    private PlantManager.Plant plantData;

    private void Start()
    {
        plantManager = FindObjectOfType<PlantManager>();
    }

    private void OnMouseDown()
    {
        if (plantData != null && plantData.isReadytoHarvest)
        {
            plantManager.CollectPlant(plantData);
        }
    }

    public void SetPlantData(PlantManager.Plant plant)
    {
        plantData = plant;
    }
}