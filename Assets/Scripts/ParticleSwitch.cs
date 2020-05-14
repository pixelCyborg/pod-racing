using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ParticleSwitch : MonoBehaviour
{
    bool activated = false;
    private ParticleSystem[] particleSystems;
    private TrailRenderer[] trailRenderers;
    private float[] trailTimes;

    // Start is called before the first frame update
    void Start()
    {
        particleSystems = GetComponentsInChildren<ParticleSystem>();
        trailRenderers = GetComponentsInChildren<TrailRenderer>();
        trailTimes = new float[trailRenderers.Length];
        for(int i = 0; i < trailRenderers.Length; i++)
        {
            trailTimes[i] = trailRenderers[0].time;
        }

        SwitchOff();
    }

    // Update is called once per frame
    public void SwitchOn()
    {
        if (activated) return;
        for(int i = 0; i < particleSystems.Length; i++)
        {
            particleSystems[i].Play();
        }
        for(int i = 0; i < trailRenderers.Length; i++)
        {
            trailRenderers[i].DOTime(trailTimes[i], 0.25f);
        }
        activated = true;
    }

    public void SwitchOff()
    {
        if (!activated) return;
        for(int i = 0; i < particleSystems.Length; i++)
        {
            particleSystems[i].Stop();
        }
        for(int i = 0; i < trailRenderers.Length; i++)
        {
            trailRenderers[i].DOTime(0, 0.25f);
        }
        activated = false;
    }
}
