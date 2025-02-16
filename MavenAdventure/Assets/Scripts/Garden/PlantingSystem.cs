using UnityEngine;
public class PlantingSystem : MonoBehaviour
{
    public BackpackManager backpackManager;
    public PlantManager plantManager;
    public LayerMask bedLayer;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 100f, bedLayer))
            {
                EmptyBed bed = hit.collider.GetComponent<EmptyBed>();

                if (bed != null && !bed.isOccupied)
                {
                    TryPlantSeed(bed);
                }
            }
        }
    }

    private void TryPlantSeed(EmptyBed bed)
    {
        if (bed.isOccupied)
        {
            Debug.Log("Bed is already occupied!");
            return; // Don't plant if the bed is occupied
        }

        if (backpackManager.selectedSeed != null)
        {
            GameObject seedling = Instantiate(backpackManager.selectedSeed.seedPrefab, bed.transform.position, Quaternion.identity);
            bed.isOccupied = true;

            // Add the plant to PlantManager
            PlantManager.Plant newPlant = new PlantManager.Plant
            {
                plantData = backpackManager.selectedSeed,
                currentStage = 0,
                currentStageObject = seedling,
                isWatered = false
            };

            plantManager.plants.Add(newPlant);
            Debug.Log($"Planted {backpackManager.selectedSeed.displayName}");

            // Remove one seed from the backpack
            backpackManager.RemoveItem(backpackManager.selectedSeed);
        }
        else
        {
            Debug.Log("No seed selected!");
        }
    }

}