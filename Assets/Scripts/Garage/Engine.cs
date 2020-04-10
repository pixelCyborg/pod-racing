using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : CarComponent
{
    public const float MIN_ACCELERATION = 0.1f;
    public const float MAX_ACCELERATION = 0.6f;

    public const float MIN_VELOCITY_DRAG = 0.95f;
    public const float MAX_VELOCITY_DRAG = 0.99f;

    [Range(MIN_ACCELERATION, MAX_ACCELERATION)]
    public float acceleration = 0.14f; //Horsepower of the engine, more acceleration = more zoom zoom
    [Range(MIN_VELOCITY_DRAG, MAX_VELOCITY_DRAG)]
    public float velocityDrag = 0.98f; //Rate at which our velocity slows down

    public Transform boostAnchor;

    public void SetBooster(Booster _booster)
    {
        ClearChildren(boostAnchor);
        SpawnToAnchor(_booster.gameObject, boostAnchor);
    }

    public void SpawnToAnchor(GameObject go, Transform anchor)
    {
        Transform newObject = Instantiate(go).transform;
        newObject.SetParent(anchor);
        newObject.localPosition = Vector3.zero;
        newObject.localRotation = Quaternion.identity;
        newObject.localScale = Vector3.one;
    }

    public void ClearChildren(Transform parent)
    {
        foreach (Transform anchor in parent)
        {
            for (int n = anchor.childCount - 1; n >= 0; n--)
            {
                Destroy(anchor.GetChild(n).gameObject);
            }
        }
    }
}
