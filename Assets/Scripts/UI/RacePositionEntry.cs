using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class RacePositionEntry : MonoBehaviour
{
    private int position;
    public Image background;
    public Image portrait;
    public Text finishTime;
    private Text text;

    private void Start()
    {
        position = transform.GetSiblingIndex() + 1;
        if(portrait == null) portrait = GetComponentInChildren<Image>();
        if(text == null) text = GetComponentInChildren<Text>();
    }

    public void UpdateEntry(string racer, Color color, float time = 0f)
    {
        text.text = position + ": " + racer;
        background.color = color;

        if(finishTime != null)
        {
            if (time == 0f) finishTime.text = "--:--:--";
            else
            {
                TimeSpan t = TimeSpan.FromSeconds(time);
                string formattedTime = string.Format("{0:D2}:{1:D2}:{2:D2}",
                t.Minutes,
                t.Seconds,
                t.Milliseconds);
                finishTime.text = formattedTime;
            }
        }
    }

    public void Hide()
    {
        text.text = "";
        background.color = Color.clear;
        portrait.enabled = false;
        if (finishTime != null) finishTime.text = "";
    }
}
