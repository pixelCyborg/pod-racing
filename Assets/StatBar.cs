using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class StatBar : MonoBehaviour
{
    public float maxValue = 0f;
    public float minValue = 1f;

    private Slider slider;
    private Text value;

    private void Start()
    {
        slider = GetComponentInChildren<Slider>();
        value = transform.Find("Value").GetComponent<Text>();
        slider.minValue = minValue;
        slider.maxValue = maxValue;
    }

    public void SetValue(float _value)
    {
        value.text = _value.ToString();
        slider.value = _value;
    }
}
