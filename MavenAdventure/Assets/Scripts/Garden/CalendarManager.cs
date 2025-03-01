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
    private Dictionary<int, Sprite> harvestDays = new Dictionary<int, Sprite>();

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

    public void MarkHarvestDay(int growthDuration, Sprite plantIcon)
    {
        int harvestDay = currentDay + growthDuration;
        if (harvestDay > totalDays)
        {
            harvestDay -= totalDays; // Wrap to next month
        }

        // Store different plant icons per day
        if (!harvestDays.ContainsKey(harvestDay))
        {
            harvestDays[harvestDay] = plantIcon;
        }

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
                // Set the color for the background image based on whether it's the current day
                dayCell.backgroundImage.color = (i + 1 == currentDay) ? currentDayColor : defaultColor;

                // Check if this day has a harvest icon and update the icon
                if (harvestDays.ContainsKey(i + 1))
                {
                    dayCell.harvestIcon.sprite = harvestDays[i + 1];
                    dayCell.harvestIcon.enabled = true;
                }
                else
                {
                    dayCell.harvestIcon.enabled = false;
                }
            }
        }
    }
}