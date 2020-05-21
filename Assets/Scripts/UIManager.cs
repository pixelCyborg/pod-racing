using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public CanvasGroup RaceUI;
    public CanvasGroup FinishUI;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        RaceStart();
    }

    public void RaceStart()
    {
        ToggleCanvasGroup(RaceUI, true);
        ToggleCanvasGroup(FinishUI, false);
    }

    public void RaceComplete()
    {
        ToggleCanvasGroup(RaceUI, false);
        ToggleCanvasGroup(FinishUI, true);
        FinishUI.GetComponent<RaceCompleteScreen>().TotalEarnings();
    }

    private void ToggleCanvasGroup(CanvasGroup group, bool enabled)
    {
        group.alpha = enabled ? 1f : 0f;
        group.interactable = enabled;
        group.blocksRaycasts = enabled;
    }
}
