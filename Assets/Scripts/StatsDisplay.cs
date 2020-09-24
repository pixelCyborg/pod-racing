using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsDisplay : MonoBehaviour
{
    public static StatsDisplay instance;

    public StatBar acceleration;
    public StatBar topSpeed;
    public StatBar boostPower;
    public StatBar boostEfficiency;
    public StatBar boostRegen;
    public StatBar turnSpeed;
    public StatBar control;

    private void Start()
    {
        instance = this;

        acceleration.SetLimits(Chassis.MIN_ACCELERATION, Chassis.MAX_ACCELERATION);
        topSpeed.SetLimits(MinSpeed(), MaxSpeed());
        boostPower.SetLimits(Engine.MIN_BOOST_FACTOR, Engine.MAX_BOOST_FACTOR);
        boostEfficiency.SetLimits(MinBoostEfficiency(), MaxBoostEfficiency());
        boostRegen.SetLimits(Engine.MIN_BOOST_REGEN, Engine.MAX_BOOST_REGEN);
        turnSpeed.SetLimits(Wing.MIN_MAX_TURN, Wing.MAX_MAX_TURN);
        control.SetLimits(Wing.MIN_TURN_DRAG, Wing.MAX_TURN_DRAG);
    }

    public void UpdateAcceleration(float accel)
    {
        acceleration.SetValue(accel);
    }

    public void UpdateMaxSpeed(float accel, float drag)
    {
        topSpeed.SetValue(SimulateMaxSpeed(accel, drag));
    }

    public void UpdateBoostPower(float factor)
    {
        boostPower.SetValue(factor);
    }

    public void UpdateBoostEfficiency(float capacity, float cost)
    {
        boostEfficiency.SetValue(capacity / cost);
    }

    public void UpdateBoostRegen(float regen)
    {
        boostRegen.SetValue(regen);
    }

    public void UpdateTurnspeed(float turn)
    {
        turnSpeed.SetValue(turn);
    }

    public void UpdateControl(float turnDrag)
    {
        control.SetValue(turnDrag);
    }

    private void SetStatLimits(StatBar statBar, float min, float max)
    {
        statBar.SetLimits(min, max);
    }

    private float MinBoostEfficiency()
    {
        return (Engine.MIN_BOOST_CAPACITY / Engine.MAX_BOOST_COST);
    }

    private float MaxBoostEfficiency()
    {
        return (Engine.MAX_BOOST_CAPACITY / Engine.MIN_BOOST_COST);
    }

    private float MinSpeed()
    {
        return SimulateMaxSpeed(Chassis.MIN_ACCELERATION, Chassis.MIN_VELOCITY_DRAG);
    }

    private float MaxSpeed()
    {
        return SimulateMaxSpeed(Chassis.MAX_ACCELERATION, Chassis.MAX_VELOCITY_DRAG);
    }

    //Brute force solution then store the value. Math is for suckers.
    private float maxSpeed = -1;
    private float SimulateMaxSpeed(float accel, float drag)
    {
        if (maxSpeed < 0)
        {
            float velocity = 0f;
            float decelleration = velocity * drag;
            int failsafe = 0;

            while (decelleration < (accel - 0.000001f) && failsafe < 2000)
            {
                velocity += accel;
                decelleration = velocity * (1f - drag);
                velocity *= drag;

                failsafe++;
            }

            maxSpeed = velocity;
        }

        float storedSpeed = maxSpeed;
        maxSpeed = -1;
        return storedSpeed;
    }
}
