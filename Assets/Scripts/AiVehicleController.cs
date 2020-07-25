using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiVehicleController : MonoBehaviour
{
    private RaceVehicle vehicle;

    private const float SLOW_UPDATE_TIME = 0.05f;
    private bool consideringBoost = false;
    private bool boosting = false;
    private float recommendedTurn = 0f;

    private void Start()
    {
        vehicle = GetComponent<RaceVehicle>();
        boosting = false;
        StartCoroutine(SlowUpdate());
    }

    // Update is called once per frame
    void Update()
    {
        if (vehicle == null) return;

        //Set vehicle controls
        recommendedTurn = DetermineTurn();

        vehicle.SetThrottle(DetermineThrottle());
        vehicle.SetBoost(ShouldBoost());

        int strafe = DetermineStrafe();
        if (strafe == -1)
        {
            //Left strafe
        }

        if (strafe == 1)
        {
            //Right strafe
        }
    }

    IEnumerator SlowUpdate()
    {
        while(true)
        {
            yield return new WaitForSeconds(SLOW_UPDATE_TIME);
            vehicle.SetTurn(recommendedTurn);
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
