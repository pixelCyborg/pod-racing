using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSpeedRTPC : MonoBehaviour

{
    public AK.Wwise.RTPC Speed;
    private RaceVehicle vehicle;

    // Start is called before the first frame update
    void Start()
    {
        vehicle = GetComponent<RaceVehicle>();
    }

    // Update is called once per frame
    void Update()
    {
        Speed.SetGlobalValue(vehicle.CurrentOutOfMaxSpeed());
    }
}
