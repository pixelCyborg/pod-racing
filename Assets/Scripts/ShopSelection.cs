using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ShopSelection : MonoBehaviour
{
    private UnityEngine.RectTransform activeSelection;
    public UnityEngine.RectTransform bodySelection;
    public UnityEngine.RectTransform engineSelection;
    public UnityEngine.RectTransform wingsSelection;
    public UnityEngine.RectTransform boostSelection;
    private Vector3 activePosition;
    private Vector3 inactivePosition;
    private float transitionSpeed = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        activeSelection = bodySelection;
        activePosition = bodySelection.anchoredPosition;
        inactivePosition = activePosition - Vector3.right * 300;

        BrowseBodies();
    }

    public void BrowseBodies()
    {
        LoadCategory(bodySelection, PartsDB.PartType.Chassis);
        ChooseSelection(bodySelection);
    }

    public void BrowseEngines()
    {
        LoadCategory(engineSelection, PartsDB.PartType.Engine);
        ChooseSelection(engineSelection);
    }

    public void BrowseWings()
    {
        LoadCategory(wingsSelection, PartsDB.PartType.Wing);
        ChooseSelection(wingsSelection);
    }

    public void BrowseBoost()
    {
        LoadCategory(boostSelection, PartsDB.PartType.Booster);
        ChooseSelection(boostSelection);
    }

    private void LoadCategory(RectTransform category, PartsDB.PartType type)
    {
        PartsDB.Part[] parts = Garage.instance.GetParts(type);
        category.GetComponent<ShopPartsCategory>().PopulateCategory(parts , type);
    }

    // Update is called once per frame
    private void ChooseSelection(UnityEngine.RectTransform newSelection)
    {
        activeSelection.DOAnchorPosX(inactivePosition.x, transitionSpeed);
        newSelection.DOAnchorPosX(activePosition.x, transitionSpeed);

        activeSelection = newSelection;
    }
}
