using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldPlanet : MonoBehaviour
{
    public Planet planet;
    public List<RaceData> potentialRaces;

    private int raceCount = 3;
    private float planetRadius = 0.5f;
    private List<GameObject> locations;

    private void Start()
    {
        Populate();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            DetailsUI.instance.ShowPlanet(planet);
            OverworldPlayer.instance.Stop();
            OverviewCamera.instance.Focus(transform.position, GetComponent<SphereCollider>());
            OverviewCamera.instance.onUnfocus.AddListener(HideLocations);
            ShowLocations();
        }
    }

    private void Populate()
    {
        locations = new List<GameObject>();
        bool merchantAdded = false;

        for(int i = 0; i < raceCount; i++)
        {
            Location newLocation = Instantiate(Overworld.instance.locationPrefab, transform).GetComponent<Location>();
            Vector3 pos = transform.position + Vector3.up * planetRadius * transform.localScale.z;

            float x, y, z;
            x = Random.Range(-360f, 360f);
            y = Random.Range(-360f, 360f);
            z = Random.Range(-360f, 360f);
            pos = RotatePointAroundPivot(pos, transform.position, new Vector3(x, y, z));
            newLocation.transform.position = pos;
            newLocation.transform.rotation = Quaternion.LookRotation(transform.position - pos);

            //Set the location info
            if(!merchantAdded)
            {
                newLocation.Initialize(Location.LocationType.Merchant);
                merchantAdded = true;
            }
            else
            {
                newLocation.Initialize(Location.LocationType.Race);
                newLocation.race = PickRace();
            }

            locations.Add(newLocation.gameObject);
        }
        HideLocations();
    }

    private RaceData PickRace()
    {
        int index = Random.Range(0, potentialRaces.Count);
        RaceData data = potentialRaces[index];
        potentialRaces.RemoveAt(index);
        return data;
    }

    public void HideLocations()
    {
        OverviewCamera.instance.onUnfocus.RemoveListener(HideLocations);
        for(int i = 0; i < locations.Count; i++)
        {
            locations[i].SetActive(false);
        }
    }

    public void ShowLocations()
    {
        for (int i = 0; i < locations.Count; i++)
        {
            locations[i].SetActive(true);
        }
    }

    public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        return Quaternion.Euler(angles) * (point - pivot) + pivot;
    }

    /*private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            DetailsUI.instance.Hide();
        }
    }*/
}

[System.Serializable]
public class Planet
{
    public string name;
    public string description;
    [HideInInspector]
    public Location[] currentEvents;
}
