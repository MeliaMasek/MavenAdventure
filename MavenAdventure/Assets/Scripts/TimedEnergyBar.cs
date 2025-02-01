using UnityEngine;
using UnityEngine.UI;
using System;

public class TimedEnergyBar : MonoBehaviour
{
    public Slider energySlider; // UI Slider for displaying energy
    public float maxEnergy = 100f; // Maximum energy
    public float depletionRate = 10f; // Energy lost per second
    public float rechargeRate = 10f; // Energy gained per second
    public float clickEnergyCost = 20f; // Energy cost per click
    public bool autoRecharge = true; // Allow auto recharge?
    public WateringInteraction wateringInteraction; // Reference to WateringInteraction

    private float currentEnergy;
    private string lastSavedTimeKey = "LastExitTime";
    private string energyKey = "EnergyValue";

    void Start()
    {
        LoadEnergyData();
        UpdateEnergyUI();
    }

    void Update()
    {
        if (currentEnergy < maxEnergy && autoRecharge)
        {
            RechargeEnergy();
        }

        UpdateEnergyUI();
    }

    void OnApplicationQuit()
    {
        SaveEnergyData();
    }

    void OnApplicationPause(bool pause)
    {
        if (pause)
            SaveEnergyData();
    }

    void RechargeEnergy()
    {
        currentEnergy += rechargeRate * Time.deltaTime;
        if (currentEnergy > maxEnergy)
        {
            currentEnergy = maxEnergy;
        }
    }

    public void ReduceEnergyOnClick()
    {
        if (currentEnergy >= clickEnergyCost)
        {
            currentEnergy -= clickEnergyCost;
            UpdateEnergyUI();
            Debug.Log("Energy reduced for watering a plant.");
        }
        else
        {
            Debug.Log("Not enough energy!");
        }
    }


    void UpdateEnergyUI()
    {
        if (energySlider != null)
        {
            energySlider.value = currentEnergy / maxEnergy;
        }
    }

    void SaveEnergyData()
    {
        PlayerPrefs.SetFloat(energyKey, currentEnergy);
        PlayerPrefs.SetString(lastSavedTimeKey, DateTime.UtcNow.ToString());
        PlayerPrefs.Save();
    }

    void LoadEnergyData()
    {
        if (PlayerPrefs.HasKey(energyKey))
        {
            currentEnergy = PlayerPrefs.GetFloat(energyKey);
        }
        else
        {
            currentEnergy = maxEnergy; // Start at full if first time playing
        }

        if (PlayerPrefs.HasKey(lastSavedTimeKey))
        {
            string lastTime = PlayerPrefs.GetString(lastSavedTimeKey);
            DateTime lastExitTime = DateTime.Parse(lastTime);
            TimeSpan timePassed = DateTime.UtcNow - lastExitTime;

            float energyRecovered = (float)timePassed.TotalSeconds * rechargeRate;
            currentEnergy = Mathf.Min(currentEnergy + energyRecovered, maxEnergy);
        }
    }
}
