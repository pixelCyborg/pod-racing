using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OverworldIcon : MonoBehaviour
{
    public static OverworldIcon instance;
    private MeshRenderer meshRend;

    private void Start()
    {
        instance = this;
        meshRend = GetComponentInChildren<MeshRenderer>();
    }

    public void Show(OverworldPlanet planet)
    {
        transform.position = planet.transform.position;
        transform.localScale = planet.transform.localScale;
        meshRend.enabled = true;
    }

    public void Hide()
    {
        meshRend.enabled = false;
    }
}
