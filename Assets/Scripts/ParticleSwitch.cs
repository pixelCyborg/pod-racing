using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSwitch : MonoBehaviour
{
    bool activated = false;
    private ParticleSystem[] particleSystems;

    // Start is called before the first frame update
    void Start()
    {
        particleSystems = GetComponentsInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    public void SwitchOn()
    {
        if (activated) return;
        for(int i = 0; i < particleSystems.Length; i++)
        {
            particleSystems[i].Play();
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
        activated = false;
    }
}
