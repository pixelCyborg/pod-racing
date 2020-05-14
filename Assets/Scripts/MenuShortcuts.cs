using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuShortcuts : MonoBehaviour
{
    public void LoadOverview()
    {
        SceneFader.instance.QueueBaseScene("Overview");
        SceneFader.instance.FadeOut();
    }

    public void LoadRace()
    {
        SceneFader.instance.QueueBaseScene("CarTest");
        SceneFader.instance.QueueAdditiveScene("BaseScene");
        SceneFader.instance.QueueAdditiveScene("Race UI");
        SceneFader.instance.FadeOut();
    }

    public void LoadGarage()
    {
        SceneFader.instance.QueueBaseScene("Garage");
        SceneFader.instance.FadeOut();
    }
}
