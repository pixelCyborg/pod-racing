using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldPlanet : MonoBehaviour
{
    public Planet planet;
    public List<RaceData> potentialRaces;
    public int planetIndex; 
    private int raceCount = 3;
    private float planetRadius = 0.5f;
    private List<GameObject> locations;
    private List<LineRenderer> lines = new List<LineRenderer>();
    public GameObject linePrefab;

    private void Start()
    {
        Populate();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Land();
        }
    }

    private void OnMouseDown()
    {
        if(!OverviewCamera.instance.zoomed) Land();
    }

    public void Land()
    {
        if(OverworldPlayer.instance) OverworldPlayer.instance.Stop();
        OverviewCamera.instance.Focus(transform.position, GetComponent<SphereCollider>());
        OverviewCamera.instance.onUnfocus.AddListener(HideLocations);
        ShowLocations();
        Overworld.instance.currentPlanetIndex = planetIndex;
        Overworld.instance.HideSolarMap();
        DetailsUI.instance.ShowPlanet(planet);
        PlacePlayer();
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
            newLocation.index = i;
            newLocation.rotCoords = new Vector3(x,y,z);

            //Set the location info
            if(!merchantAdded && i == raceCount - 1)
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

        for(int i = 0; i < locations.Count; i++)
        {
            if(i < locations.Count - 1)
            {
                ConnectLocations(locations[i].transform.localPosition, locations[i + 1].transform.localPosition);
            }
        } 
        HideLocations();
    }

    public void ConnectLocations(Vector3 pointA, Vector3 pointB)
    {
        List<Vector3> points = new List<Vector3>();
        int positionCount = 25;
        for(int i = 0; i < positionCount; i++)
        {
            float t = (float)i / (float)positionCount;
            points.Add(Vector3.Slerp(pointA, pointB, t) * transform.localScale.x * 1.02f + transform.position);
        }
        points.Add(pointB * transform.localScale.x * 1.02f + transform.position);

        LineRenderer lineRend = SpawnLineRend();
        lines.Add(lineRend);
        lineRend.positionCount = points.Count;
        lineRend.widthMultiplier = 0.005f * transform.localScale.x;
        lineRend.SetPositions(points.ToArray());
        
    }

    private LineRenderer SpawnLineRend()
    {
        GameObject go = Instantiate(linePrefab);
        return go.GetComponent<LineRenderer>();
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
        for(int i = 0; i < lines.Count; i++)
        {
            lines[i].enabled = false;
        }
    }

    public void ShowLocations()
    {
        for (int i = 0; i < locations.Count; i++)
        {
            locations[i].SetActive(true);
        }
        for (int i = 0; i < lines.Count; i++)
        {
            lines[i].enabled = true;
        }
    }

    public void PlacePlayer()
    {
        if(Location.currentLocation != null && GameObject.Equals(Location.currentLocation.transform.parent.gameObject, gameObject))
        {
            Location.currentLocation.Open();
        }
        else if(SaveLoadSystem.CurrentLocationIndex() > -1)
        {
            locations[SaveLoadSystem.CurrentLocationIndex()].GetComponent<Location>()?.Open();
        }
        else
        {
            locations[0].GetComponent<Location>()?.Open();
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
