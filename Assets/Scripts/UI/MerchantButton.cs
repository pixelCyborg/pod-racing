using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MerchantButton : MonoBehaviour
{
    public PartsDB.Part part;

    public void Initialize(PartsDB.Part _part, bool equipped = false)
    {
        Button button = GetComponent<Button>();
        part = _part;

        GetComponentInChildren<Text>().text = part.prefab.name;
    }

    public void Select()
    {
        MerchantWindow.instance.SelectPart(part);
        ShopPreview.instance.SetPreview(part.prefab);
    }
}
