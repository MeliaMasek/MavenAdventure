using UnityEngine;

public class PlantInteraction : MonoBehaviour
{
    public PlantManager.Plant plantData;
    public BackpackManager backpackManager;

    private void Start()
    {
        if (backpackManager == null)
        {
            backpackManager = FindObjectOfType<BackpackManager>();
        }
    }

    private void OnMouseDown()
    {
        if (plantData == null || plantData.currentStageObject == null)
        {
            return;
        }

        ProduceData produceData = plantData.produceData;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == null) return;

        if (other.CompareTag("WateringCan"))
        {
            if (plantData != null)
            {
                plantData.isWatered = true;
            }
        }
        else if (other.CompareTag("PlantBed"))
        {
            if (backpackManager.GetSelectedSeed() == null)
            {
                return;
            }
            PlantSelectedSeed(other.transform);
        }
    }

    private void PlantSelectedSeed(Transform planterLocation)
    {
        InventoryData selectedSeed = backpackManager.GetSelectedSeed();

        PlantManager.Plant newPlant = new PlantManager.Plant
        {
            plantData = selectedSeed,
            basePrefab = selectedSeed.seedPrefab, // Use the seed prefab
            spawnLocator = planterLocation,
            spawnScale = Vector3.one
        };

        FindObjectOfType<PlantManager>().plants.Add(newPlant);
        FindObjectOfType<PlantManager>().SetStage(newPlant, 0); // Plant the seed

        backpackManager.RemoveItem(selectedSeed);

        backpackManager.ToggleBackpack(false);
    }
}
