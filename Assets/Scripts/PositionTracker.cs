
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionTracker : MonoBehaviour
{
    public static List<RaceVehicle> vehicles;
    private RacePositionEntry[] positions;

    private void Start()
    {
        positions = new RacePositionEntry[transform.childCount];

        for(int i = 0; i < positions.Length; i++)
        {
            positions[i] = transform.GetChild(i).GetComponent<RacePositionEntry>();
        }
    }

    private void Update()
    {
        UpdatePositions();
    }

    public void UpdatePositions()
    {
        if (vehicles == null || vehicles.Count < 1) return;

        for(int i = 0; i < positions.Length; i++)
        {
            if (i < vehicles.Count)
            {
                positions[i].UpdateEntry(vehicles[i].name, vehicles[i].isPlayer ? Color.white : Color.clear, vehicles[i].GetTotalRaceTime());
            }
            else
            {
                positions[i].Hide();
            }
        }
    }
}
