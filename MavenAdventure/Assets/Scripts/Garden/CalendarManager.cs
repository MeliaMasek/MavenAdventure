
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CalendarManager : MonoBehaviour
{
    public GameObject dayCellPrefab;
    public Transform calendarGrid;
    public int totalDays = 30;
    public Color defaultColor;
    public Color currentDayColor;

    public string[] seasons = { "Spring", "Summer", "Fall", "Winter" };
    public Text seasonText;

    private int currentDay = 1;
    private int currentMonth = 1;
    private int currentSeasonIndex = 0;
    private GameObject[] dayCells;

    // Dictionary to store multiple harvests per day with different plants
    private Dictionary<int, List<Sprite>> harvestDays = new Dictionary<int, List<Sprite>>();
    private Dictionary<int, int> harvestMonths = new Dictionary<int, int>(); // Tracks which month the harvest happens in

    void Start()
    {
        GenerateCalendar();
        UpdateCalendarUI();
    }

    void GenerateCalendar()
    {
        dayCells = new GameObject[totalDays];

        for (int i = 0; i < totalDays; i++)
        {
            GameObject newCell = Instantiate(dayCellPrefab, calendarGrid);
            newCell.GetComponentInChildren<Text>().text = (i + 1).ToString();
            dayCells[i] = newCell;
        }
    }

    public int GetCurrentDay()
    {
        return currentDay;
    }

    public void AdvanceDay()
    {
        currentDay++;

        if (currentDay > totalDays)
        {
            currentDay = 1;
            currentMonth++;

            if (currentMonth > 12) 
                currentMonth = 1;

            // Change season every 3 months
            if (currentMonth == 1 || currentMonth == 4 || currentMonth == 7 || currentMonth == 10)
            {
                currentSeasonIndex = (currentSeasonIndex + 1) % seasons.Length;
            }
        }

        UpdateCalendarUI();
    }

    public void MarkHarvestDay(int plantingDay, int growthDuration, Sprite plantIcon, InventoryData selectedSeed)
    {
        // Use the daysToSprout from the selected seed data
        int sproutDuration = selectedSeed.daysToSprout;

        // Calculate the total harvest day
        int harvestDay = plantingDay + growthDuration + sproutDuration;
        int harvestMonth = currentMonth;

        // Handle overflow to the next month
        while (harvestDay > totalDays)
        {
            harvestDay -= totalDays; // Wrap to the next month
            harvestMonth++; // Move to the next month
            if (harvestMonth > 12) harvestMonth = 1; // Wrap around for a new year
        }

        // Ensure the dictionary entry exists
        if (!harvestDays.ContainsKey(harvestDay))
        {
            harvestDays[harvestDay] = new List<Sprite>();
            harvestMonths[harvestDay] = harvestMonth; // Store the month as well
        }

        // Add the plant icon to the harvestDays dictionary
        harvestDays[harvestDay].Add(plantIcon);

        Debug.Log($"Plant: {selectedSeed.displayName}, Planted on: {plantingDay}, Growth: {growthDuration}, Sprout Duration: {sproutDuration}, Final Harvest Day: {harvestDay}, Harvest Month: {harvestMonth}");

        UpdateCalendarUI();
    }
    
    void UpdateCalendarUI()
    {
        seasonText.text = seasons[currentSeasonIndex];

        for (int i = 0; i < totalDays; i++)
        {
            CalendarDayCell dayCell = dayCells[i].GetComponent<CalendarDayCell>();

            if (dayCell != null)
            {
                // Change background color for the current day
                dayCell.backgroundImage.color = (i + 1 == currentDay) ? currentDayColor : defaultColor;

                // Debug log to check which days have harvests
                if (harvestDays.ContainsKey(i + 1))
                {
                    Debug.Log($"Updating Day {i + 1}: Showing {harvestDays[i + 1].Count} harvest icons");
                    
                    // Ensure we're displaying the correct icons only for the current month
                    if (harvestMonths[i + 1] == currentMonth)
                    {
                        dayCell.ShowMultipleIcons(harvestDays[i + 1]); 
                    }
                    else
                    {
                        // Clear icons if the harvest is from a different month
                        dayCell.ShowMultipleIcons(new List<Sprite>());
                    }
                }
                else
                {
                    // If no harvests, clear previous icons
                    dayCell.ShowMultipleIcons(new List<Sprite>());
                }
            }
        }
    }
}
