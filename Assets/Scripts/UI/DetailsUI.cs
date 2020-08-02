using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DetailsUI : MonoBehaviour
{
    public CanvasGroup planetUI;
    public CanvasGroup raceUI;

    public static DetailsUI instance;
    public RaceData raceData;
    Vector3 origScale;
    float animTime = 0.33f;

    public Text planet;
    public Text planetDescription;
    public Text courseName;
    public Text courseDescription;
    public Text rewards;

    public Button deployButton;

    private void Start()
    {
        instance = this;
        deployButton.interactable = false;
        origScale = transform.localScale;
        //transform.localScale = Vector3.zero;
        Overworld.instance.Move += Hide;
        ToggleFader(raceUI, false, 0f);
        ToggleFader(planetUI, false, 0f);
    }

    /*
    public void Show(Planet planet)
    {
        Show(new RaceData("Default", 1, 2000), planet);
    }

    public void Show(RaceData race, Planet planetData)
    {
        planet.text = planetData.name;
        planetDescription.text = planetData.description;

        raceData = race;

        courseName.text = race.courseName;
        rewards.text = "Prize: " + race.prizePool;

        transform.DOScale(origScale, 0.2f).SetEase(Ease.OutCubic);
    }
    */

    public void ShowPlanet(Planet planetData)
    {
        planet.text = planetData.name;
        planetDescription.text = planetData.description;

        ToggleFader(planetUI, true);
    }

    public void ShowLocation(Location location)
    {
        if (location.race == null) {
            location.race = new RaceData("Default", 1, 2000);
        }

        raceData = location.race;
        courseName.text = raceData.courseName;
        rewards.text = "Prize: " + raceData.prizePool;

        deployButton.interactable = true;
        ToggleFader(raceUI, true);
    }

    void ToggleFader(CanvasGroup fader, bool enabled, float transitionTime = 0.2f)
    {
        fader.DOFade(enabled ? 1.0f : 0.0f, transitionTime).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            fader.interactable = enabled;
            fader.blocksRaycasts = enabled;
        });
    }

    public void Hide(Vector3 target)
    {
        Hide();
    }

    public void Hide()
    {
        HidePlanet();
        HideRace();
        OverviewCamera.instance.UnFocus();
    }

    public void HidePlanet()
    {
        ToggleFader(planetUI, false);
    }

    public void HideRace()
    {
        deployButton.interactable = false;
        ToggleFader(raceUI, false);
        OverviewCamera.instance.ShiftPlanet(false);
    }
}
