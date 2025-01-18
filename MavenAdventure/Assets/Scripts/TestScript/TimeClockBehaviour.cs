using UnityEngine;

//Code borrowed from https://pastebin.com/Mj1K5E64 and ChatGPT
public class TimeClockBehaviour : MonoBehaviour
{
    public class Clock
    {
        private System.DateTime now;
        private System.TimeSpan timeNow;
        private System.TimeSpan gameTime;
        private int minutesPerDay; // Realtime minutes per game-day (1440 would be realtime)

        public Clock(int minPerDay)
        {
            minutesPerDay = minPerDay;
        }

        public System.DateTime GetTime()
        {
            now = System.DateTime.Now;
            timeNow = now.TimeOfDay;
            double hours = timeNow.TotalMinutes % minutesPerDay;
            double minutes = (hours % 1) * 60;
            gameTime = new System.TimeSpan((int)hours, (int)minutes, 0); // Set seconds to 0

            // Construct a DateTime object with the time components
            System.DateTime time = now.Date.Add(gameTime);

            return time;
        }
    }

    Clock clock;

    void Start()
    {
        clock = new Clock(24);
    }

    void FixedUpdate()
    {
        //Debug.Log(clock.GetTime().ToString("hh:mm")); // Format to show only hours and minutes
    }
}