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
        vehicle.LoadPlayerVehicle();
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
        Accountant.AddValue("Placement", CheckWinnings());
        UIManager.instance.RaceComplete();
        vehicle.SetBoost(false);
        gameObject.AddComponent<AiVehicleController>();
        this.enabled = false;
    }

    int CheckWinnings()
    {
        int winnings = 0;

        for (int i = 0; i < PositionTracker.vehicles.Count; i++)
        {
            if(GameObject.ReferenceEquals(PositionTracker.vehicles[i].gameObject, vehicle.gameObject))
            {
                
                //1st
                if (i == 0)
                {
                    winnings = RaceManager.raceData.prizePool;
                }
                //2nd
                else if(i == 1)
                {
                    winnings = (int)(RaceManager.raceData.prizePool * 0.5f);
                }
                //3rd
                else if(i == 2)
                {
                    winnings = (int)(RaceManager.raceData.prizePool * 0.25f);
                }
                else
                {
                    //nothin
                }
            }
        }

        return winnings;
    }
}
