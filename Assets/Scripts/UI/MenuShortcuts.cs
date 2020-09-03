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
        RaceManager.raceData = DetailsUI.instance.raceData;

        SceneFader.instance.QueueBaseScene(DetailsUI.instance.raceData.courseName);
        SceneFader.instance.QueueAdditiveScene("BaseScene");
        //SceneFader.instance.QueueAdditiveScene("Race UI");
        SceneFader.instance.FadeOut();
    }

    public void LoadGarage()
    {
        SceneFader.instance.QueueBaseScene("Garage");
        SceneFader.instance.FadeOut();
    }

    public void AddMoney(int amount)
    {
        CreditsTracker.AddCredits(amount);
    }

    public void ResetMoney()
    {
        CreditsTracker.AddCredits(-CreditsTracker.Credits());
    }

    public void ClearSave()
    {
        SaveLoadSystem.ClearSave();
    }
}
