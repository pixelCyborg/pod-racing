using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldPlanet : MonoBehaviour
{
    public Planet planet;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            DetailsUI.instance.Show(planet);
            OverworldPlayer.instance.Stop();
            OverviewCamera.instance.Focus(transform.position);
        }
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
    public RaceData[] currentEvents;
}
