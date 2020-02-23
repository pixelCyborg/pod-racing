using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVehicleController : MonoBehaviour
{
    private RaceVehicle vehicle;

    private void Start()
    {
        vehicle = GetComponent<RaceVehicle>();
        vehicle.isPlayer = true;
        RaceTracker.instance.trackedVehicle = vehicle;
        vehicle.OnRaceComplete.AddListener(PlayerRaceComplete);
    }

    // Update is called once per frame
    void Update()
    {
        if (vehicle == null) return;

        //Set vehicle controls
        vehicle.SetTurn(Input.GetAxis("Horizontal"));
        vehicle.SetThrottle(Input.GetAxis("Vertical"));
        vehicle.SetBoost(Input.GetButton("Boost"));
        vehicle.SetDrift(Input.GetButton("Drift"));
        vehicle.SetStrafe(Input.GetAxis("Strafe"));

        /*
        if (Input.GetButtonDown("LStrafe"))
        {
            vehicle.LStrafe();
        }

        if (Input.GetButtonDown("RStrafe"))
        {
            vehicle.RStrafe();
        }
        */

        vehicle.UpdatePathPosition(VertPathHelper.instance.GetPathTime(transform));
        vehicle.UpdateUI();
    }

    void PlayerRaceComplete()
    {
        UIManager.instance.RaceComplete();
        gameObject.AddComponent<AiVehicleController>();
        this.enabled = false;
    }
}
