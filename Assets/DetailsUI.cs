using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DetailsUI : MonoBehaviour
{
    public static DetailsUI instance;
    public RaceData raceData;
    Vector3 origScale;
    float animTime = 0.33f;

    public Text planet;
    public Text planetDescription;
    public Text courseName;
    public Text courseDescription;
    public Text rewards;

    private void Start()
    {
        instance = this;
        origScale = transform.localScale;
        transform.localScale = Vector3.zero;
        Overworld.instance.Move += Hide;
    }

    public void Show()
    {
        Show(new RaceData("Default", 1, 2000));
    }

    public void Show(RaceData race)
    {
        raceData = race;

        courseName.text = race.courseName;
        rewards.text = "Prize: " + race.prizePool;

        transform.DOScale(origScale, 0.2f).SetEase(Ease.OutCubic);
    }

    public void Hide(Vector3 target)
    {
        Hide();
    }

    public void Hide()
    {
        transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InCubic);
        OverviewCamera.instance.UnFocus();
    }
}
