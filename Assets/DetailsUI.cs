using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DetailsUI : MonoBehaviour
{
    public static DetailsUI instance;
    Vector3 origScale;
    float animTime = 0.33f;

    private void Start()
    {
        instance = this;
        origScale = transform.localScale;
        transform.localScale = Vector3.zero;
        Overworld.instance.Move += Hide;
    }

    public void Show()
    {
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
