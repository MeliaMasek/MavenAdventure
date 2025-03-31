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

            if (currentMonth > 12) currentMonth = 1;

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

        // Calculate harvest day based on planting day, growth duration, and sprout duration
        int harvestDay = plantingDay + growthDuration + sproutDuration;

        // Handle overflow to the next month (wraparound)
        if (harvestDay > totalDays)
        {
            harvestDay -= totalDays; // Wrap to next month
        }

        // Ensure the dictionary entry exists
        if (!harvestDays.ContainsKey(harvestDay))
        {
            harvestDays[harvestDay] = new List<Sprite>();
        }

        // Add the plant icon to the harvestDays dictionary
        harvestDays[harvestDay].Add(plantIcon);

        Debug.Log($"Plant: {selectedSeed.displayName}, Planted on: {plantingDay}, Growth: {growthDuration}, Sprout Duration: {sproutDuration}, Final Harvest Day: {harvestDay}");

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
                // Change background color
                dayCell.backgroundImage.color = (i + 1 == currentDay) ? currentDayColor : defaultColor;

                // If there are harvests for this day, show all icons
                if (harvestDays.ContainsKey(i + 1))
                {
                    dayCell.ShowMultipleIcons(harvestDays[i + 1]); 
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