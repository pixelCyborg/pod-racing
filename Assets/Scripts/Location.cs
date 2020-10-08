using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Location : MonoBehaviour
{
    public static Location currentLocation;
    [HideInInspector] 
    public RaceData race;
    LayerMask markerMask;
    public LocationType type;
    public Transform locationMarker;
    public MeshRenderer playerMarker;
    Vector3 origScale;
    bool hovered, prevHovered;

    public Vector3 rotCoords;
    public int index;

    public enum LocationType
    {
        Race, Merchant
    }

    public void Initialize(LocationType _type)
    {
        playerMarker.enabled = false;
        origScale = locationMarker.localScale;
        type = _type;

        if(type == LocationType.Merchant)
        {
            SetColor(Color.green);
        }
        else
        {
            SetColor(Color.white);
        }
    }

    public void SetColor(Color color)
    {
        MeshRenderer[] rends = GetComponentsInChildren<MeshRenderer>();
        for(int i = 0; i < rends.Length; i++)
        {
            Color newColor = color;
            newColor.a = rends[i].material.color.a;
            rends[i].material.SetColor("_BaseColor", newColor);
        }
    }

    private void Start()
    {
        markerMask = 1 << LayerMask.NameToLayer("Marker");
    }

    private void Update()
    {
        if (OverviewCamera.instance.rotDisabled) return;


        hovered = false;
        //Mouse Hover
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100f, markerMask))
        {
            if (GameObject.Equals(hit.transform.gameObject, gameObject))
            {
                locationMarker.DOKill();
                if (!hovered) locationMarker.DOScale(origScale * 1.2f, 0.33f);
                hovered = true;
            }
        }
        if(prevHovered && !hovered)
        {
            locationMarker.DOKill();
            locationMarker.DOScale(origScale, 0.33f);
        }
        prevHovered = hovered;
        //=============

        //On Click
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100f, markerMask))
            {
                if(GameObject.Equals(hit.transform.gameObject, gameObject))
                {
                    Open();
                }
            }
        }
    }

    public void Open()
    {
        if (currentLocation != null) currentLocation.playerMarker.enabled = false;

        Debug.Log("Location Clicked!");
        playerMarker.enabled = true;
        OverworldPlanet planet = transform.GetComponentInParent<OverworldPlanet>();
        currentLocation = this;

        if (type == LocationType.Merchant)
        {
            MerchantWindow.instance.Toggle(true);
            DetailsUI.instance.HidePlanet();
            DetailsUI.instance.HideRace();
        }
        else if (type == LocationType.Race)
        {
            OverviewCamera.instance.ShiftPlanet(true, planet.transform.localScale.x * 0.5f);
            DetailsUI.instance.ShowLocation(this);
        }

        RotateCameraToView();
    }

    public void RotateCameraToView()
    {
        OverviewCamera.instance.ViewLocation(rotCoords);
    }

    public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        return Quaternion.Euler(angles) * (point - pivot) + pivot;
    }
}
