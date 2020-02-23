using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapMarker : MonoBehaviour
{
    private void Start()
    {
        Color color = GetComponent<SpriteRenderer>().color;
        color.a = 1.0f;
        GetComponent<SpriteRenderer>().color = color;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rot = new Vector3(90, 0, 0);
        rot.y = transform.parent.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Euler(rot);
    }
}
