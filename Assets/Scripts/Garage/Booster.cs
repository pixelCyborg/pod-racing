using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : CarComponent
{
    public float boostFactor = 1.2f; //Just a multiplier of how much the boost increases our acceleration. The power of the booster
    public float boostCost = 25; //How much the booster costs to keep going. This should increase along with the boostFactor
    public float boostCapacity = 100; //How large our boost tank is, this should usually be 100
    public float boostRegen = 15; //How fast we get our boost meter back when not boosting
}
