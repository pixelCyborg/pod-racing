using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ShopSelection : MonoBehaviour
{
    private RectTransform activeSelection;
    public RectTransform bodySelection;
    public RectTransform engineSelection;
    public RectTransform wingsSelection;
    public RectTransform boostSelection;
    private Vector3 activePosition;
    private Vector3 inactivePosition;
    private float transitionSpeed = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        activeSelection = bodySelection;
        activePosition = bodySelection.anchoredPosition;
        inactivePosition = activePosition - Vector3.right * 300;
    }

    public void BrowseBodies()
    {
        ChooseSelection(bodySelection);
    }

    public void BrowseEngines()
    {
        ChooseSelection(engineSelection);
    }

    public void BrowseWings()
    {
        ChooseSelection(wingsSelection);
    }

    public void BrowseBoost()
    {
        ChooseSelection(boostSelection);
    }

    // Update is called once per frame
    private void ChooseSelection(RectTransform newSelection)
    {
        activeSelection.DOAnchorPosX(inactivePosition.x, transitionSpeed);
        newSelection.DOAnchorPosX(activePosition.x, transitionSpeed);

        activeSelection = newSelection;
    }
}
