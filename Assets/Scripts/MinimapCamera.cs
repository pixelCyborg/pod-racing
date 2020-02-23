using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerVehicleController>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.position + Vector3.up * 1000f;
        Vector3 rot = transform.rotation.eulerAngles;
        rot.y = player.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Euler(rot);
    }
}
