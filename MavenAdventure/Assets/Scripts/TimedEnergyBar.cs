using UnityEngine;
using UnityEngine.UI; // Import UI

public class TimedEnergyBar : MonoBehaviour
{
    public int maxEnergy = 100;  // Maximum energy
    public int currentEnergy;    // Current energy level
   public int reduceEnergyRate = 10; // Energy reduced per click
    public float rechargeRate = 10f; // Energy gained per hour (real-time)
    
    public Slider energySlider;  // UI Slider for visual energy bar
    public Text energyText;      // UI Text to display energy amount

    private void Start()
    {
        LoadEnergy(); // Load saved energy when game starts
        UpdateEnergyUI();
    }

    private void Update()
    {
        RegenerateEnergy();
    }

    public void ReduceEnergyOnClick()
    {
        if (currentEnergy > 0)
        {
            currentEnergy -= reduceEnergyRate; // Reduce energy by a set amount
            SaveEnergy();
            UpdateEnergyUI();
        }
    }

    private void RegenerateEnergy()
    {
        float hoursPassed = (float)(System.DateTime.Now - LoadLastPlayedTime()).TotalHours;
        int energyToRestore = Mathf.FloorToInt(hoursPassed * rechargeRate);

        if (energyToRestore > 0)
        {
            currentEnergy = Mathf.Clamp(currentEnergy + energyToRestore, 0, maxEnergy);
            SaveEnergy();
            UpdateEnergyUI();
        }
    }

    private void UpdateEnergyUI()
    {
        energySlider.value = (float)currentEnergy / maxEnergy;
        energyText.text = currentEnergy.ToString(); // Update number display
    }

    private void SaveEnergy()
    {
        PlayerPrefs.SetInt("SavedEnergy", currentEnergy);
        PlayerPrefs.SetString("LastPlayedTime", System.DateTime.Now.ToString());
        PlayerPrefs.Save();
    }

    private void LoadEnergy()
    {
        currentEnergy = PlayerPrefs.GetInt("SavedEnergy", maxEnergy);
    }

    private System.DateTime LoadLastPlayedTime()
    {
        string savedTime = PlayerPrefs.GetString("LastPlayedTime", System.DateTime.Now.ToString());
        return System.DateTime.Parse(savedTime);
    }
}