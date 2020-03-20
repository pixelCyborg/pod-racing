using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wing : CarComponent
{
    public float turnSpeed = 0.5f; //This stuff is similar to strafe
    public float maxTurn = 1.6f; //We don't want to turn too fast, or it'll confuse the player
    public float turnDrag = 0.66f; //Decellerate turn very quickly
}
