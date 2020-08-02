using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Location : MonoBehaviour
{
    [HideInInspector] 
    public RaceData race;
    LayerMask markerMask;
    public LocationType type;

    public enum LocationType
    {
        Race, Merchant
    }

    public void Initialize(LocationType _type)
    {
        type = _type;

        if(type == LocationType.Merchant)
        {
            SetColor(Color.green);
        }
    }

    public void SetColor(Color color)
    {
        MeshRenderer[] rends = GetComponentsInChildren<MeshRenderer>();
        for(int i = 0; i < rends.Length; i++)
        {
            rends[i].material.SetColor("_BaseColor", color);
        }
    }

    private void Start()
    {
        markerMask = 1 << LayerMask.NameToLayer("Marker");
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100f, markerMask))
            {
                if(GameObject.Equals(hit.transform.gameObject, gameObject))
                {
                    Open();
                }
            }
        }
    }

    private void Open()
    {
        Debug.Log("Location Clicked!");
        OverworldPlanet planet = transform.GetComponentInParent<OverworldPlanet>();

        if (type == LocationType.Merchant)
        {
            MerchantWindow.instance.Toggle(true);
        }
        else if (type == LocationType.Race)
        {
            OverviewCamera.instance.ShiftPlanet(true, planet.transform.localScale.x * 0.5f);
            DetailsUI.instance.ShowLocation(this);
        }
    }
}
