using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : CarComponent
{
    public float acceleration = 0.14f; //Horsepower of the engine, more acceleration = more zoom zoom
    public float velocityDrag = 0.98f; //Rate at which our velocity slows down
}
