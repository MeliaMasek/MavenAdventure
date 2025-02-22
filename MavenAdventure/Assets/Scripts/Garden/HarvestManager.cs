using UnityEngine;

public class HarvestManager : MonoBehaviour
{
    public BackpackManager backpackManager;
    public PlantManager plantManager;
    private void Start()
    {
        backpackManager = FindObjectOfType<BackpackManager>();
        plantManager = FindObjectOfType<PlantManager>();
    }

    public void HarvestPlant(PlantManager.Plant plant, ProduceData produceData)
    {
        if (produceData == null)
        {
            Debug.LogError("Harvest failed: ProduceData is null!");
            return;
        }

        Debug.Log($"Harvesting {produceData.displayName}...");

        backpackManager.AddProduceToBackpack(produceData);
    }

}