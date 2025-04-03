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
    
    public LayerMask plantLayer;  // Declare the LayerMask variable

    public WateringInteraction wateringInteraction;

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
        if (currentEnergy.value >= reduceEnergyRate.value)
        {
            currentEnergy.value -= reduceEnergyRate.value;
            SaveEnergy();
            UpdateEnergyUI();

            // Call the TryWaterPlant method from the WateringInteraction script
            if (wateringInteraction != null)
            {
                // Assuming 'clickedObject' is the object you want to water, this would be passed as a parameter
                // Replace clickedObject with the correct object you are interacting with
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 100f, plantLayer))
                {
                    wateringInteraction.TryWaterPlant(hit.collider.gameObject);
                }
            }
        }
        else
        {
            Debug.Log("Not enough energy to complete this task.");
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