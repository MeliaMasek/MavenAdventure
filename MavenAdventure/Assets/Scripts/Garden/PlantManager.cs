using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlantManager : MonoBehaviour
{
    [System.Serializable]
    public class Plant
    {
        public GameObject baseStage;      // Base stage (e.g., dirt)
        public GameObject seedStage;      // Seed stage GameObject
        public GameObject sproutStage;    // Sprout stage GameObject
        public GameObject matureStage;    // Mature stage GameObject

        public int daysToSprout = 2;      // Days required to transition to sprout
        public int daysToMature = 5;      // Days required to transition to mature

        [HideInInspector] public int currentStage = -1;   // -1 = Base, 0 = Seed, 1 = Sprout, 2 = Mature
        [HideInInspector] public int daysElapsed = 0;     // Days elapsed since planting
        [HideInInspector] public bool isWatered = false;  // Whether the plant has been watered today
        [HideInInspector] public bool isFertilized = false; // Whether the plant has been fertilized

        public Vector3 initialPosition; // Store the initial position
    }

    public List<Plant> plants = new List<Plant>();

    private void Start()
    {
        foreach (var plant in plants)
        {
            plant.initialPosition = plant.baseStage.transform.position; // Store the position
            SetStage(plant, -1); // Set the plant's initial stage
        }
    }

    public void EndDay()
    {
        foreach (var plant in plants)
        {
            if (plant.isWatered)
            {
                plant.daysElapsed++;

                int sproutDays = plant.isFertilized ? Mathf.Max(1, plant.daysToSprout - 1) : plant.daysToSprout;
                int matureDays = plant.isFertilized ? Mathf.Max(1, plant.daysToMature - 1) : plant.daysToMature;

                if (plant.currentStage == -1 && plant.daysElapsed >= sproutDays)
                {
                    SetStage(plant, 0);
                }
                else if (plant.currentStage == 0 && plant.daysElapsed >= sproutDays)
                {
                    SetStage(plant, 1);
                }
                else if (plant.currentStage == 1 && plant.daysElapsed >= matureDays)
                {
                    SetStage(plant, 2);
                }
            }
            else
            {
                Debug.Log($"Plant is not watered and will not grow: {plant}");
            }

            plant.isWatered = false;
            plant.isFertilized = false;
        }
    }

    public void WaterPlant(Plant plant)
    {
        if (!plant.isWatered)
        {
            plant.isWatered = true;
            Debug.Log($"Watered the plant: {plant.baseStage.name}");
        }
        else
        {
            Debug.Log($"The plant {plant.baseStage.name} is already watered today.");
        }
    }

    private void SetStage(Plant plant, int stage)
    {
        plant.currentStage = stage;

        Vector3 currentPosition = plant.baseStage.transform.position;

        plant.baseStage.SetActive(stage == -1);
        plant.seedStage.SetActive(stage == 0);
        plant.sproutStage.SetActive(stage == 1);
        plant.matureStage.SetActive(stage == 2);

        plant.baseStage.transform.position = currentPosition;
        plant.seedStage.transform.position = currentPosition;
        plant.sproutStage.transform.position = currentPosition;
        plant.matureStage.transform.position = currentPosition;

        if (stage < 2)
        {
            plant.daysElapsed = 0;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        // Check if the dropped object is the watering can
        var draggedObject = eventData.pointerDrag;

        if (draggedObject != null && draggedObject.CompareTag("WateringCan"))
        {
            // Find the plant being dropped on
            foreach (var plant in plants)
            {
                if (IsPointerOverPlant(eventData, plant))
                {
                    WaterPlant(plant);
                    return;
                }
            }

            Debug.Log("The watering can was dropped, but not over a plant.");
        }
        else
        {
            Debug.Log("Invalid object dropped.");
        }
    }

    private bool IsPointerOverPlant(PointerEventData eventData, Plant plant)
    {
        // Check if the pointer is over the plant's collider
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.collider.gameObject == plant.baseStage ||
                   hit.collider.gameObject == plant.seedStage ||
                   hit.collider.gameObject == plant.sproutStage ||
                   hit.collider.gameObject == plant.matureStage;
        }
        return false;
    }
}
