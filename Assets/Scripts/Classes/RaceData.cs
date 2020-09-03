using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RaceData
{
    public int laps;
    public string courseName;
    public int prizePool;

    public RaceData(string _courseName, int _laps, int _prizePool)
    {
        courseName = _courseName;
        laps = _laps;
        prizePool = _prizePool;
    }
}

public class RacerProfile
{
    public string name;
    public string description;
    public string portrait;
    private Sprite portraitSprite;
    public VehicleData vehicle;
}
