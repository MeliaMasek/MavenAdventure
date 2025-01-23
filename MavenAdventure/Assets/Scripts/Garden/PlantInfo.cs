using UnityEngine;
using UnityEngine.UI;

public class PlantInfo : MonoBehaviour
{
    public GameObject infoPopup;      // The popup panel
    public Text plantNameText;        // Text element for plant name
    public Text currentStageText;     // Text element for current stage
    public Text nextStageText;        // Text element for next stage
    public Text daysRemainingText;    // Text element for days remaining
    //public Text waterStatusText;      // Text element for water status
    //public Text fertilizerStatusText; // Text element for fertilizer status

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
        currentPlant = plant;

        // Update UI text elements with plant data
        plantNameText.text = "Plant Name: " + (plant.baseStage.name ?? "Unknown");
        currentStageText.text = "Current Stage: " + GetStageName(plant.currentStage);
        nextStageText.text = "Next Stage: " + GetStageName(plant.currentStage + 1);
        daysRemainingText.text = "Days Remaining: " + GetDaysRemaining(plant);
        //waterStatusText.text = "Watered Today: " + (plant.isWatered ? "Yes" : "No");
        //fertilizerStatusText.text = "Fertilized Today: " + (plant.isFertilized ? "Yes" : "No");

        // Show the popup
        infoPopup.SetActive(true);
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
        if (plant.currentStage == -1)
            return plant.daysToSprout - plant.daysElapsed;
        else if (plant.currentStage == 0)
            return plant.daysToSprout - plant.daysElapsed;
        else if (plant.currentStage == 1)
            return plant.daysToMature - plant.daysElapsed;
        else
            return 0; // No further stages
    }
}
