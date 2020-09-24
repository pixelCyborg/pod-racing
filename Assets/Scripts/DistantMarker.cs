using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistantMarker : MonoBehaviour
{
    private float minAlphaDistance = 300f;
    private float maxAlphaDistance = 5000f;
    Transform camTransform;
    SpriteRenderer sprite;
    Vector3 origScale;

    // Start is called before the first frame update
    void Start()
    {
        camTransform = Camera.main.transform;
        sprite = GetComponent<SpriteRenderer>();
        origScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        float scaleMulti = 0f;
        float distance = Vector3.Distance(transform.position, camTransform.position);
        if (distance > minAlphaDistance)
        {
            transform.LookAt(camTransform);
            scaleMulti = (float)(distance - 10) / (float)(maxAlphaDistance - 10);
            if (scaleMulti > 1.0f) scaleMulti = 1.0f;
        }

        transform.localScale = origScale * scaleMulti;
        //sprite.color = color;
    }
}
