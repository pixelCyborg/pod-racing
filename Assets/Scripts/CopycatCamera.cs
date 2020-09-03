using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopycatCamera : MonoBehaviour
{
    public Camera parentCam;
    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        cam.fieldOfView = parentCam.fieldOfView;
    }
}
