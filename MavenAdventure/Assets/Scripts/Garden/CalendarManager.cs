using UnityEngine;
using UnityEngine.UI;

public class CalendarManager : MonoBehaviour
{
    public GameObject dayCellPrefab; // Prefab for each calendar day
    public Transform calendarGrid; // Parent object (Grid Layout)
    public int totalDays = 30; // Days per month
    public Color defaultColor;
    public Color currentDayColor;

    public string[] seasons = { "Spring", "Summer", "Fall", "Winter" }; // Array for seasons
    public Text seasonText; // To display only the season

    private int currentDay = 1;
    private int currentMonth = 1; // Month counter (1 to 12)
    private int currentSeasonIndex = 0; // Index for seasons array
    private GameObject[] dayCells;

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

    public void AdvanceDay()
    {
        currentDay++;

        // Check if month is over and reset day
        if (currentDay > totalDays)
        {
            Debug.Log("Month ended. Resetting day and changing season.");
            currentDay = 1;
            currentMonth++;

            // Check if we reached a new year (i.e., 12 months)
            if (currentMonth > 12)
            {
                currentMonth = 1; // Reset month to January
            }

            // Switch season every 3 months
            if (currentMonth == 1 || currentMonth == 4 || currentMonth == 7 || currentMonth == 10) 
            {
                currentSeasonIndex = (currentSeasonIndex + 1) % seasons.Length;
                Debug.Log("Season changed to: " + seasons[currentSeasonIndex]);
            }
        }

        UpdateCalendarUI();
    }

    void UpdateCalendarUI()
    {
        // Log for debugging
        Debug.Log("Updating UI. Current Season: " + seasons[currentSeasonIndex] + ", Day: " + currentDay);

        // Update UI text for season
        seasonText.text = seasons[currentSeasonIndex]; // Update season UI element

        // Update day cell colors
        for (int i = 0; i < totalDays; i++)
        {
            CalendarDayCell dayCell = dayCells[i].GetComponent<CalendarDayCell>(); // Get custom script
            if (dayCell != null && dayCell.backgroundImage != null)
            {
                dayCell.backgroundImage.color = (i + 1 == currentDay) ? currentDayColor : Color.white;
            }
        }
    }
}
