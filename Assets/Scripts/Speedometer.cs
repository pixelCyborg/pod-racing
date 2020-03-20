using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Speedometer : MonoBehaviour
{
    public static Speedometer instance;
    public Text speedText;

    private void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    public void SetSpeed(float currentSpeed, float maxSpeed = -1)
    {
        string text = string.Format("{0:0.00}", currentSpeed * 10);
        if (maxSpeed > -1) text = text + " / " + string.Format("{0:0.00}", maxSpeed * 10);
        speedText.text = text;
    }

    public void SetStrafe(float currentSpeed)
    {
        //speedText.text = currentSpeed + " mph";
    }
}
