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
    public void SetSpeed(float currentSpeed)
    {
        speedText.text = string.Format("{0:0.00}", currentSpeed * 10);
    }

    public void SetStrafe(float currentSpeed)
    {
        //speedText.text = currentSpeed + " mph";
    }
}
