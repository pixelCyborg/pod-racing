using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistantMarker : MonoBehaviour
{
    private float minAlphaDistance = 300f;
    private float maxAlphaDistance = 1000f;
    Transform camTransform;
    SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        camTransform = Camera.main.transform;
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Color color = sprite.color;
        float alpha = 0f;
        float distance = Vector3.Distance(transform.position, camTransform.position);
        if (distance > minAlphaDistance)
        {
            transform.LookAt(camTransform);
            alpha = (distance - 10) / (maxAlphaDistance - 10);
            if (alpha > 1.0f) alpha = 1.0f;
        }

        color.a = alpha;
        sprite.color = color;
    }
}
