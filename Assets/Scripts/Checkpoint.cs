using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool isFinishLine = false;

    private void OnTriggerEnter(Collider other)
    {
        RaceVehicle vehicle = other.GetComponentInParent<RaceVehicle>();
        if (vehicle != null)
        {
            float angle = Vector3.Angle(transform.forward, vehicle.CurrentVelocity());
            if (angle < 90 && angle > -90)
            {
                vehicle.Lap();
            }
        }
    }
}
