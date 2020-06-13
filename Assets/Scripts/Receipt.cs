using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Receipt : MonoBehaviour
{
    public Text description;
    public Text value;

    public void SetPayment(string _description, int _value)
    {
        gameObject.SetActive(true);
        description.text = _description;
        value.text = _value.ToString();

        if(_value > 0)
        {
            description.color = Color.green;
            value.color = Color.green;
            value.text = "+" + value.text;
        }
        else if(_value < 0)
        {
            description.color = Color.red;
            value.color = Color.red;
            value.text = "-" + value.text;
        }
        else
        {
            description.color = Color.grey;
            value.color = Color.grey;
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
