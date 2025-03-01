
using UnityEngine;
using UnityEngine.UI;

public class WateringInteraction : MonoBehaviour
{
    public bool isWateringMode = false;
    public LayerMask plantLayer;
    public PlantManager plantManager;
    public BackpackManager backpackManager;
    public Material dirtWet;
    public Material dirtDry;

    public TimedEnergyBar energyBar;
    public Button wateringButton;

    private void Start()
    {
        if (plantManager == null)
        {
            plantManager = FindObjectOfType<PlantManager>();
        }

        if (backpackManager == null)
        {
            backpackManager = FindObjectOfType<BackpackManager>();
        }
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && isWateringMode)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 100f, plantLayer))
            {
                foreach (var plant in plantManager.plants)
                {
                    if (plant.currentStageObject == hit.collider.gameObject)
                    {
                        if (plant.currentStage == 2)
                        {
                            CollectPlant(plant);
                            return;
                        }
                        else if (isWateringMode)
                        {
                            bool watered = TryWaterPlant(hit.collider.gameObject);
                            if (watered)
                            {
                                isWateringMode = false;
                                wateringButton.GetComponent<Image>().color = Color.white;
                            }
                        }
                    }
                }
            }
        }
    }

    public void ActivateWateringMode()
    {
        if (!HasPlantedSeeds())
        {
            return;
        }

        FindObjectOfType<SeedManager>().isPlantingMode = false;
        isWateringMode = !isWateringMode;
    }
    
    public void DeactivateWateringMode()
    {
        isWateringMode = false;
        wateringButton.GetComponent<Image>().color = Color.white;
    }
    
    private void CollectPlant(PlantManager.Plant plant)
    {
        if (plant.currentStage == 2)
        {
            if (backpackManager != null)
            {
                backpackManager.AddToBackpack(plant.plantData);
            }
            
            Destroy(plant.currentStageObject);
            plantManager.plants.Remove(plant);
        }
    }

    private bool TryWaterPlant(GameObject clickedObject)
    {
        foreach (var plant in plantManager.plants)
        {
            if (plant.currentStageObject == clickedObject)
            {
                if (plant.currentStage == -1)
                {
                    return false; 
                }

                if (!plant.isWatered) 
                {
                    plant.isWatered = true; 
                    ChangeDirtMaterial(plant.currentStageObject);
                    energyBar.ReduceEnergyOnClick();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        return false; 
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
                    meshRenderer.material = dirtWet;
                    materialChanged = true;
                }
            }
        }
    }

    public bool HasUnwateredPlants()
    {
        foreach (var plant in plantManager.plants)
        {
            if (!plant.isWatered && plant.currentStage != -1)
            {
                return true; 
            }
        }
        return false;
    }
    
    private bool HasPlantedSeeds()
    {
        foreach (var plant in plantManager.plants)
        {
            if (plant.currentStage >= 0)
            {
                return true;
            }
        }
        return false; 
    }
}
