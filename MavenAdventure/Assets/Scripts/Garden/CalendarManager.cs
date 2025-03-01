using UnityEngine;
using UnityEngine.UI;

public class CalendarManager : MonoBehaviour
{
    public GameObject dayCellPrefab; // Prefab for each calendar day
    public Transform calendarGrid; // Parent object (Grid Layout)
    public int totalDays = 30; // Days per month
    public Color defaultColor;
    public Color currentDayColor;

    private int currentDay = 1;
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
        if (currentDay > totalDays) currentDay = 1; // Reset month
        UpdateCalendarUI();
    }

    void UpdateCalendarUI()
    {
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