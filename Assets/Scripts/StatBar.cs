using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class StatBar : MonoBehaviour
{
    private Slider slider;
    private Text value;

    private void Awake()
    {
        slider = GetComponentInChildren<Slider>();
        value = transform.Find("Value").GetComponent<Text>();
    }

    public void SetLimits(float min, float max)
    {
        slider.minValue = min;
        slider.maxValue = max;
    }

    public void SetValue(float _value)
    {
        slider.DOValue(_value, 0.5f);
        value.text = _value.ToString();
    }
}
