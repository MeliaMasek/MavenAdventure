using UnityEngine;
using UnityEngine.UI;

//Code borrowed from ChatGPT

public class ClockUI : MonoBehaviour
{
    public Text timeText;
    private TimeClockBehaviour.Clock clock;

    void Start()
    {
        clock = new TimeClockBehaviour.Clock(24);
    }

    void FixedUpdate()
    {
        UpdateTime();
    }

    void UpdateTime()
    {
        System.DateTime time = clock.GetTime();
        timeText.text = string.Format("{0:D2}:{1:D2}", time.Hour, time.Minute);
    }
}