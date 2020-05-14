using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedLinesOverlay : MonoBehaviour
{
    private static ParticleSystem particles;

    // Start is called before the first frame update
    void Start()
    {
        particles = GetComponent<ParticleSystem>();
    }

    public static void On()
    {;
        particles.Play();
    }

    public static void Off()
    {
        particles.Stop();
    }
}
