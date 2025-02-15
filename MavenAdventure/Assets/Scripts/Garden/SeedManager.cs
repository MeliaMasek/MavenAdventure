using UnityEngine;

public class SeedManager : MonoBehaviour
{
    public bool isPlantingMode = false;
    public LayerMask plantLayer;
    public PlantManager plantManager;
    public Material dirtDry;

    public void ActivatePlantingMode()
    {
        isPlantingMode = true;
    }

    private void Update()
    {
        if (isPlantingMode && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 100f, plantLayer.value))
            {
                foreach (var plant in plantManager.plants)
                {
                    if (plant.currentStageObject == hit.collider.gameObject)  
                    {
                        if (plant.currentStage == -1)
                        {
                            PlantBed plantBed = hit.collider.GetComponentInParent<PlantBed>();
                            if (plantBed != null)
                            {
                                PlantSeed(plant, plantBed);
                            }
                        }
                        isPlantingMode = false;
                        break;
                    }
                }
            }
        }
    }

    public void PlantSeed(PlantManager.Plant oldPlant, PlantBed plantBed)
    {
        InventoryData selectedSeed = FindObjectOfType<BackpackManager>().GetSelectedSeed();
        if (selectedSeed == null)
        {
            return;
        }

        if (selectedSeed.seedPrefab == null)
        {
            return;
        }

        if (selectedSeed.sproutPrefab == null || selectedSeed.maturePrefab == null)
        {
            return;
        }

        Destroy(oldPlant.currentStageObject);

        PlantManager.Plant newPlant = new PlantManager.Plant
        {
            plantData = selectedSeed,
            seedPrefab = selectedSeed.seedPrefab,
            sproutPrefab = selectedSeed.sproutPrefab,
            maturePrefab = selectedSeed.maturePrefab,
            spawnLocator = oldPlant.spawnLocator,
            spawnScale = Vector3.one
        };

        newPlant.currentStageObject = Instantiate(newPlant.seedPrefab, newPlant.spawnLocator.position, Quaternion.identity);
        newPlant.currentStage = 0;

        plantManager.plants.Remove(oldPlant);
        plantManager.plants.Add(newPlant);
        
        FindObjectOfType<BackpackManager>().RemoveItem(selectedSeed);
    }
    
    private void ChangeDirtMaterial(GameObject stageObject)
    {
        Transform[] childTransforms = stageObject.GetComponentsInChildren<Transform>();

        bool materialChanged = false;

        foreach (Transform child in childTransforms)
        {
            if (child.CompareTag("Dirt"))
            {
                MeshRenderer meshRenderer = child.GetComponent<MeshRenderer>();

                if (meshRenderer != null)
                {
                    meshRenderer.material = dirtDry; // Change to wet material
                    materialChanged = true;
                }
            }
        }
    }
}