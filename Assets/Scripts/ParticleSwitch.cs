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
    private float[] trailWidths;

    // Start is called before the first frame update
    void Start()
    {
        particleSystems = GetComponentsInChildren<ParticleSystem>();
        trailRenderers = GetComponentsInChildren<TrailRenderer>();
        trailTimes = new float[trailRenderers.Length];
        trailWidths = new float[trailRenderers.Length];
        for(int i = 0; i < trailRenderers.Length; i++)
        {
            trailTimes[i] = trailRenderers[0].time;
            trailWidths[i] = trailRenderers[0].startWidth;
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
            trailRenderers[i].enabled = true;
            trailRenderers[i].DOTime(trailTimes[i], 0.25f);
            //DOTween.To(() => trailRenderers[i].startWidth, x => trailRenderers[i].startWidth = x, trailWidths[i], 1f);
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
            trailRenderers[i].DOTime(trailTimes[i] * 0.5f, 0.25f);
            //DOTween.To(() => trailRenderers[i].startWidth, x => trailRenderers[i].startWidth = x, 0f, 1f);
        }
        activated = false;
    }
}
