using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public RaceVehicle targetRacer;
    private Transform target;
    public float followSpeed = 10.0f;
    public float rotateSpeed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        if(targetRacer != null)
        {
            target = targetRacer.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            transform.position = target.position;
            transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, Time.deltaTime * rotateSpeed);
        }
    }
}
