using UnityEngine;
using UnityEngine.UI;

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

        if (currentDay > totalDays)
        {
            currentDay = 1;
            currentMonth++;

            if (currentMonth > 12)
            {
                currentMonth = 1;
            }

            if (currentMonth == 1 || currentMonth == 4 || currentMonth == 7 || currentMonth == 10) 
            {
                currentSeasonIndex = (currentSeasonIndex + 1) % seasons.Length;
            }
        }

        UpdateCalendarUI();
    }

    void UpdateCalendarUI()
    {
        seasonText.text = seasons[currentSeasonIndex];

        for (int i = 0; i < totalDays; i++)
        {
            CalendarDayCell dayCell = dayCells[i].GetComponent<CalendarDayCell>();
            if (dayCell != null && dayCell.backgroundImage != null)
            {
                dayCell.backgroundImage.color = (i + 1 == currentDay) ? currentDayColor : Color.white;
            }
        }
    }
}
