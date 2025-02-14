using UnityEngine;
using UnityEngine.UI;

public class PlantInfo : MonoBehaviour
{
    public GameObject infoPopup;      // The popup panel
    public Text plantNameText;        // Text element for plant name
    public Text currentStageText;     // Text element for current stage
    public Text nextStageText;        // Text element for next stage
    public Text daysRemainingText;    // Text element for days remaining

    private PlantManager.Plant currentPlant;

    private void Start()
    {
        // Ensure the popup is hidden at the start
        if (infoPopup != null)
        {
            infoPopup.SetActive(false);
        }
    }

    public void ShowPlantInfo(PlantManager.Plant plant)
    {
        if (plant == null)
        {
            Debug.LogWarning("No plant provided to ShowPlantInfo.");
            return;
        }

        currentPlant = plant;

        // Update UI text elements with plant data
        plantNameText.text = "Plant Name: " + (plant.basePrefab?.name ?? "Unknown");
        currentStageText.text = "Current Stage: " + GetStageName(plant.currentStage);
        nextStageText.text = "Next Stage: " + GetStageName(plant.currentStage + 1);
        daysRemainingText.text = "Days Remaining: " + GetDaysRemaining(plant);

        // Show the popup
        if (infoPopup != null)
        {
            infoPopup.SetActive(true);
        }
    }

    public void ClosePopup()
    {
        // Hide the popup
        if (infoPopup != null)
        {
            infoPopup.SetActive(false);
        }
    }

    private string GetStageName(int stage)
    {
        switch (stage)
        {
            case -1: return "Base";
            case 0: return "Seed";
            case 1: return "Sprout";
            case 2: return "Mature";
            default: return "Unknown";
        }
    }

    private int GetDaysRemaining(PlantManager.Plant plant)
    {
        if (plant == null)
        {
            Debug.LogWarning("Plant is null in GetDaysRemaining.");
            return 0;
        }

        int remainingDays = 0;

        switch (plant.currentStage)
        {
            case -1:
                //remainingDays = plant.daysToSprout - plant.daysElapsed;
                break;
            case 0:
                //remainingDays = plant.daysToSprout - plant.daysElapsed;
                break;
            case 1:
                //remainingDays = plant.daysToMature - plant.daysElapsed;
                break;
            default:
                remainingDays = 0;
                break;
        }

        return Mathf.Max(0, remainingDays); // Ensure non-negative values
    }
}
