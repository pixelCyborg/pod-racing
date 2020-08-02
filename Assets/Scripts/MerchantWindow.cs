using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MerchantWindow : MonoBehaviour
{
    public static MerchantWindow instance;
    public float fadeTime = 0.2f;
    public Text itemText;
    public Text itemDescription;
    public GameObject garagePartButton;
    public Button buyButton;

    private List<PartsDB.Part> componentInventory;
    private ScrollRect scrollRect;
    private PartsDB.Part currentSelection;
    private CanvasGroup group;
    private StatsDisplay stats;

    private void Start()
    {
        instance = this;
        scrollRect = GetComponentInChildren<ScrollRect>();
        stats = GetComponentInChildren<StatsDisplay>();
        GenerateInventory();
        group = GetComponent<CanvasGroup>();
        group.alpha = 0f;
        group.interactable = false;
        group.blocksRaycasts = false;
        ClearSelection();
    }

    public void Toggle(bool enabled)
    {
        if(!enabled)
        {
            group.interactable = false;
            group.blocksRaycasts = false;
        }

        group.DOFade(enabled ? 1f : 0f, fadeTime).OnComplete(() =>
        {
            if(enabled)
            {
                group.interactable = true;
                group.blocksRaycasts = true;
            }
        });
    }

    public void GenerateInventory()
    {
        componentInventory = new List<PartsDB.Part>();
        int amount = Random.Range(3, 7);

        for(int i = 0; i < amount; i++)
        {
            componentInventory.Add(PartsCollection.Instance.GetRandomPart());
        }

        PopulateMerchant(componentInventory.ToArray());
    }

    public void BuyPart()
    {
        BuyPart(currentSelection);
    }

    public void BuyPart(PartsDB.Part part)
    {
        Garage.instance.AddPart(part);
        componentInventory.Remove(part);
        PopulateMerchant(componentInventory.ToArray());
    }

    public void SelectPart(PartsDB.Part part)
    {
        itemText.text = part.prefab.name;
        currentSelection = part;
        buyButton.interactable = true;
        //stats
    }

    public void ClearSelection()
    {
        itemText.text = null;
        currentSelection = null;
        buyButton.interactable = false;
    }

    public void PopulateMerchant(PartsDB.Part[] components)
    {
        ClearCategory();

        for (int i = 0; i < components.Length; i++)
        {
            GameObject button = Instantiate(garagePartButton, scrollRect.content);
            button.GetComponent<MerchantButton>().Initialize(components[i]);
        }
    }

    public void ClearCategory()
    {
        for (int i = scrollRect.content.childCount - 1; i >= 0; i--)
        {
            Destroy(scrollRect.content.GetChild(i).gameObject);
        }
    }
}
