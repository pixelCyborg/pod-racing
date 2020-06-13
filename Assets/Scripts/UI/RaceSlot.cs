using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceSlot : MonoBehaviour
{
    // Start is called before the first frame update
    public void SpawnRacer(GameObject vehicle)
    {
        RaceVehicle raceVehicle = Instantiate(vehicle, transform.position + (transform.up * 2f), transform.rotation).GetComponent<RaceVehicle>();
    }
}
