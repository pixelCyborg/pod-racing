using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostPad : MonoBehaviour
{
    public float boostFactor = 1.33f;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        RaceVehicle vehicle = other.GetComponentInParent<RaceVehicle>();
        if(vehicle)
        {
            Debug.Log("Boosting!");
            vehicle.Boost(boostFactor);
        }
    }
}
