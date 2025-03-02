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

    public void MarkHarvestDay(int growthDuration, Sprite plantIcon)
    {
        int harvestDay = currentDay + growthDuration;
        if (harvestDay > totalDays)
        {
            harvestDay -= totalDays; // Wrap to next month
        }

        // Find the correct calendar cell
        GameObject dayCell = dayCells[harvestDay - 1];

        // Get or create the HarvestIconContainer
        Transform iconContainer = dayCell.transform.Find("HarvestGrid");
        if (iconContainer == null)
        {
            GameObject newContainer = new GameObject("HarvestGrid");
            newContainer.transform.SetParent(dayCell.transform);
            newContainer.AddComponent<RectTransform>();
            GridLayoutGroup grid = newContainer.AddComponent<GridLayoutGroup>();

            // Configure Grid Layout
            grid.cellSize = new Vector2(50, 50); // Adjust size as needed
            grid.spacing = new Vector2(5, 5);
            grid.childAlignment = TextAnchor.MiddleCenter;

            iconContainer = newContainer.transform;
        }

        // Create and add the icon to the container
        GameObject newIcon = new GameObject("HarvestIcon");
        newIcon.transform.SetParent(iconContainer);
        Image iconImage = newIcon.AddComponent<Image>();
        iconImage.sprite = plantIcon;
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