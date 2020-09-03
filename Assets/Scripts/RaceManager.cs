using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RaceManager : MonoBehaviour
{
    public static RaceData raceData;
    private int currentLap = 0;

    public static RaceManager instance;
    public int countdownTime = 3;

    [HideInInspector]
    public float startTime;
    [HideInInspector]
    public List<float> lapTimes;
    [HideInInspector]
    public bool raceStarted;

    [HideInInspector]
    public List<RaceVehicle> racers;

    [HideInInspector]
    public UnityEvent OnRaceComplete;

    public RaceVehicle defaultRacer;
    public int participants;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        SpawnRacers();
        racers = new List<RaceVehicle>(FindObjectsOfType<RaceVehicle>());
        lapTimes = new List<float>();
        raceStarted = false;
        StartCoroutine(StartCountdown());
    }

    private void Update()
    {
        racers.Sort((a, b) => a.RacePosition().CompareTo(b.RacePosition()));
        racers.Reverse();
        PositionTracker.vehicles = racers;
    }

    private void SpawnRacers()
    {
        if (defaultRacer == null) return;

        RaceSlot[] slots = FindObjectsOfType<RaceSlot>();
        for(int i = 0; i < participants; i++)
        {
            slots[i].SpawnRacer(defaultRacer.gameObject);
        }
    }

    IEnumerator StartCountdown()
    {
        RaceTracker.instance.Countdown();
        int countdown = countdownTime;
        while (countdown > 0)
        {
            countdown--;
            yield return new WaitForSeconds(1.0f);
        }
        startTime = Time.time;
        raceStarted = true;
    }

    public void Lap()
    {
        Debug.Log("Crossed checkpoint");
        currentLap++;
        lapTimes.Add(CurrentLapTime());
        if (currentLap > raceData.laps) FinishRace();
    }

    public int CurrentLap()
    {
        if (currentLap < 1) return 1;
        if (currentLap > raceData.laps) return raceData.laps;
        return currentLap;
    }

    public void FinishRace()
    {
        Debug.Log("Race complete!");
    }

    public float ElapsedTime()
    {
        return Time.time - startTime;
    }

    public float CurrentLapTime()
    {
        if (lapTimes != null && lapTimes.Count > 0)
        {
            return Time.time - lapTimes[lapTimes.Count - 1] - startTime;
        }
        else if (!raceStarted) return 0f;
        else return ElapsedTime();
    }
}
