using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSunLocation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Shader.SetGlobalVector("_SunDirection", transform.forward);
    }
}
