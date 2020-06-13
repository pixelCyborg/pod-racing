using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostMeter : MonoBehaviour
{
    private Slider slider;
    public static BoostMeter instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        slider = GetComponent<Slider>();
    }

    public void SetBoost(float currentFuel, float totalFuel)
    {
        slider.value = currentFuel / totalFuel;
    }
}
