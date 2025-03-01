using UnityEngine;
using UnityEngine.UI;

public class TimedEnergyBar : MonoBehaviour
{
    public IntData currentEnergy;
    public IntData maxEnergy;
    public IntData reduceEnergyRate;
    public float rechargeRate;

    public Slider energySlider;  
    public Text energyText;      

    private void Start()
    {
        LoadEnergy();
        RegenerateEnergy(); 
        UpdateEnergyUI();
    }

    private void Update()
    {
        RegenerateEnergy();
    }

    public void ReduceEnergyOnClick()
    {
        if (currentEnergy.value > 0)
        {
            currentEnergy.value -= reduceEnergyRate.value; 
            SaveEnergy();
            UpdateEnergyUI();
        }
    }

    private void RegenerateEnergy()
    {
        System.DateTime lastTime = LoadLastPlayedTime();
        float minutesPassed = (float)(System.DateTime.Now - lastTime).TotalMinutes;
        int energyToRestore = Mathf.FloorToInt(minutesPassed * rechargeRate);

        if (energyToRestore > 0)
        {
            currentEnergy.value = Mathf.Clamp(currentEnergy.value + energyToRestore, 0, maxEnergy.value);
            SaveEnergy();
        }

        UpdateEnergyUI();
    }
    
    private void UpdateEnergyUI()
    {
        energySlider.value = (float)currentEnergy.value / maxEnergy.value;
        energyText.text = currentEnergy.value.ToString();
    }
    private void SaveEnergy()
    {
        PlayerPrefs.SetInt("SavedEnergy", currentEnergy.value);
        PlayerPrefs.SetString("LastPlayedTime", System.DateTime.Now.ToString());
        PlayerPrefs.Save();
    }

    private void LoadEnergy()
    {
        currentEnergy.value = PlayerPrefs.GetInt("SavedEnergy", maxEnergy.value);
    }

    private System.DateTime LoadLastPlayedTime()
    {
        string savedTime = PlayerPrefs.GetString("LastPlayedTime", System.DateTime.Now.ToString());
        return System.DateTime.Parse(savedTime);
    }
}