using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldPlanet : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            DetailsUI.instance.Show();
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
