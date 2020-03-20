using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VehicleData
{
    public float weight; //This increases max speed but decreases acceleration
    public float handling; //Increases turn speed, strafe speed

    //Throttle / Main Engine
    public float acceleration = 0.14f; //Horsepower of the engine, more acceleration = more zoom zoom
    public float velocityDrag = 0.98f; //Rate at which our velocity slows down

    //Booster
    public float boostFactor = 1.2f; //Just a multiplier of how much the boost increases our acceleration. The power of the booster
    public float boostCost = 25; //How much the booster costs to keep going. This should increase along with the boostFactor
    public float boostCapacity = 100; //How large our boost tank is, this should usually be 100
    public float boostRegen = 15; //How fast we get our boost meter back when not boosting

    //Side Thrusters - should this be it's own thing?
    public float strafeSpeed = 0.4f; //The acceleration of our strafe, this should generally be fairly high to help strafing feel smooth
    public float maxStrafe = 4f; //Usually this is how fast we're strafing, it shouldn't take long to hit this
    public float strafeDrag = 0.85f; //Keep strafe velocity degenning at a pretty high rate, otherwise we'll drift everywhere

    //Wings
    public float turnSpeed = 0.5f; //This stuff is similar to strafe
    public float maxTurn = 1.6f; //We don't want to turn too fast, or it'll confuse the player
    public float turnDrag = 0.66f; //Decellerate turn very quickly

    //Location in the Reource folder of component prefabs
    public string chassis;
    public string wing;
    public string engine;
    public string booster;

    public VehicleData()
    {
        chassis = "Car Pieces/Chassis/Med Chassis";
        engine = "Car Pieces/Engine/Round Engine";
        wing = "Car Pieces/Wings/Angled Wing";
    }
}
