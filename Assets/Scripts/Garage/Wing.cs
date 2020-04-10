using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wing : CarComponent
{
    public const float MIN_TURN_SPEED = 0.33f;
    public const float MAX_TURN_SPEED = 0.66f;

    public const float MIN_MAX_TURN = 1.0f;
    public const float MAX_MAX_TURN = 2.0f;

    public const float MIN_TURN_DRAG = 0.4f;
    public const float MAX_TURN_DRAG = 0.9f;

    [Range(MIN_TURN_SPEED, MAX_TURN_SPEED)]
    public float turnSpeed = 0.5f; //This stuff is similar to strafe
    [Range(MIN_MAX_TURN, MAX_MAX_TURN)]
    public float maxTurn = 1.6f; //We don't want to turn too fast, or it'll confuse the player
    [Range(MIN_TURN_DRAG, MAX_TURN_DRAG)]
    public float turnDrag = 0.66f; //Decellerate turn very quickly
}
