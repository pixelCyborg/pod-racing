using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarterSlots : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        Transform startAnchor = GameObject.FindGameObjectWithTag("StartPosition")?.transform;
        if(startAnchor != null)
        {
            transform.SetPositionAndRotation(startAnchor.position, startAnchor.rotation);
        }
    }
}
