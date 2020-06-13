using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPartsCategory : MonoBehaviour
{
    public GameObject garagePartButton;
    private ScrollRect scrollRect;

    private void Start()
    {
        scrollRect = GetComponentInChildren<ScrollRect>();
    }

    public void PopulateCategory(PartsDB.Part[] components, PartsDB.PartType partType)
    {
        ClearCategory();

        for(int i = 0; i < components.Length; i++)
        {
            GameObject button = Instantiate(garagePartButton, scrollRect.content);
            button.GetComponent<ShopPartButton>().Initialize(components[i].prefab, partType);
        }
    }

    public void ClearCategory()
    {
        for(int i = scrollRect.content.childCount - 1; i >= 0; i--)
        {
            Destroy(scrollRect.content.GetChild(i).gameObject);
        }
    }
}
