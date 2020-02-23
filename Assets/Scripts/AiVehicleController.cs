using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiVehicleController : MonoBehaviour
{
    private RaceVehicle vehicle;

    private bool consideringBoost = false;
    private bool boosting = false;

    private void Start()
    {
        vehicle = GetComponent<RaceVehicle>();
        boosting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (vehicle == null) return;

        //Set vehicle controls
        vehicle.SetTurn(DetermineTurn());
        vehicle.SetThrottle(DetermineThrottle());
        vehicle.SetBoost(ShouldBoost());
        vehicle.SetDrift(false);

        int strafe = DetermineStrafe();
        if (strafe == -1)
        {
            vehicle.LStrafe();
        }

        if (strafe == 1)
        {
            vehicle.RStrafe();
        }
    }

    float DetermineThrottle()
    {
        return 1f;
    }

    float DetermineTurn()
    {
        VertPathHelper.TrackInfo info = VertPathHelper.instance.GetNextPointDirection(transform);
        vehicle.UpdatePathPosition(info.pathTime);
        return info.turn;
    }

    bool ShouldBoost()
    {
        if (!boosting && !consideringBoost) StartCoroutine(ConsiderBoosting());
        if (boosting && vehicle.currentFuel < 5) boosting = false;
        return boosting;
    }

    IEnumerator ConsiderBoosting()
    {
        consideringBoost = true;
        yield return new WaitForSeconds(1.0f);
        while (!boosting)
        {
            if (vehicle.currentFuel > 25f || vehicle.currentFuel >= vehicle.boostCapacity)
            {
                if (Random.Range(0, 100) > 75) boosting = true;
            }
            yield return new WaitForSeconds(1.0f);
        }
        consideringBoost = false;
    }

    int DetermineStrafe()
    {
        return 0;
    }
}
