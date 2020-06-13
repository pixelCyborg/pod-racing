using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaceTracker : MonoBehaviour
{
    public static RaceTracker instance;

    public Text countdownTimer;
    public Text elapsedTimeDisplay;
    public Text lapCounter;
    public UnityEngine.RectTransform lapTimes;
    public RaceVehicle trackedVehicle;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    public void Countdown()
    {
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        int i = RaceManager.instance.countdownTime;
        countdownTimer.enabled = true;
        while(i > 0)
        {
            i--;
            countdownTimer.text = (i + 1).ToString();
            yield return new WaitForSeconds(1.0f);
        }
        countdownTimer.text = "GO";
        yield return new WaitForSeconds(1.5f);
        countdownTimer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTime();
        UpdateLaps();
        UpdateLapTimes();
    }

    void UpdateTime()
    {
        TimeSpan t = TimeSpan.FromSeconds(trackedVehicle.CurrentLapTime());

        string time = string.Format("{0:D2}:{1:D2}:{2:D2}",
                        t.Minutes,
                        t.Seconds,
                        t.Milliseconds);

        elapsedTimeDisplay.text = time;
    }

    void UpdateLaps()
    {
        int lap = trackedVehicle.GetCurrentLap();

        if (RaceManager.raceData == null) return;
        if (lap == RaceManager.raceData.laps) lapCounter.text = "Final Lap";
        else lapCounter.text = trackedVehicle.GetCurrentLap() + " / " + RaceManager.raceData.laps;
    }

   void UpdateLapTimes()
    {
        int lapsNoted = 0;
        for(int i = lapTimes.childCount - 1; i >= 0; i-- )
        {
            Text text = lapTimes.GetChild(i).GetComponent<Text>();
            if (lapsNoted < trackedVehicle.lapTimes.Count)
            {
                TimeSpan t = TimeSpan.FromSeconds(trackedVehicle.lapTimes[lapsNoted]);
                string time = string.Format("{0:D2}:{1:D2}:{2:D2}",
                t.Minutes,
                t.Seconds,
                t.Milliseconds);

                text.enabled = true;
                text.text = (1 + lapsNoted) + ". " + time;

                lapsNoted++;
            }
            else
            {
                text.enabled = false;
            }
        }
    }
}
