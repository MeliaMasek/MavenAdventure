using UnityEngine;

public class SeedManager : MonoBehaviour
{
    public bool isPlantingMode = false;
    public LayerMask plantLayer;
    public PlantManager plantManager;
    public Material dirtDry;

    public void ActivatePlantingMode()
    {
        FindObjectOfType<WateringInteraction>().DeactivateWateringMode();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (isPlantingMode && Physics.Raycast(ray, out hit, 100f, plantLayer.value))
            {
                foreach (var plant in plantManager.plants)
                {
                    if (plant.currentStageObject == hit.collider.gameObject)  
                    {
                        if (plant.currentStage == -1 && plant.currentStageObject.CompareTag("PlantBed"))  
                        {
                            PlantBed plantBed = hit.collider.GetComponentInParent<PlantBed>();
                            if (plantBed != null)
                            {
                                PlantSeed(plant, plantBed);
                            }
                        }
                        isPlantingMode = false;
                        return;
                    }
                }
            }

            if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("Harvest")))
            {
                PlantManager.Plant plant = plantManager.plants.Find(p => p.currentStageObject == hit.collider.gameObject);
                if (plant != null)
                {
                    plantManager.CollectPlant(plant);
                }
            }
        }
    }

    public void PlantSeed(PlantManager.Plant oldPlant, PlantBed plantBed)
    {
        if (oldPlant.currentStage != -1)
        {
            return;
        }

        InventoryData selectedSeed = FindObjectOfType<BackpackManager>().GetSelectedSeed();
        if (selectedSeed == null) return;
        if (selectedSeed.seedPrefab == null || selectedSeed.sproutPrefab == null || selectedSeed.maturePrefab == null) return;

        oldPlant.baseStageObject = oldPlant.currentStageObject;

        if (oldPlant.baseStageObject != null && oldPlant.baseStageObject.CompareTag("PlantBed"))
        {
            oldPlant.baseStageObject.SetActive(false);
        }

        PlantManager.Plant newPlant = new PlantManager.Plant
        {
            plantData = selectedSeed,
            seedPrefab = selectedSeed.seedPrefab,
            sproutPrefab = selectedSeed.sproutPrefab,
            maturePrefab = selectedSeed.maturePrefab,
            spawnLocator = oldPlant.spawnLocator,
            spawnScale = Vector3.one,
            baseStageObject = oldPlant.baseStageObject
        };

        newPlant.currentStageObject = Instantiate(newPlant.seedPrefab, newPlant.spawnLocator.position, Quaternion.identity);
        newPlant.currentStage = 0;

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
                    meshRenderer.material = dirtDry;
                    materialChanged = true;
                }
            }
        }
    }
}