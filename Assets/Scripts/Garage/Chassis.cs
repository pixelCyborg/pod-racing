using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chassis : CarComponent
{
    public Transform engineParent;
    public Transform wingParent;

    public void SetEngine(Engine engine)
    {
        ClearChildren(engineParent);

        for(int i = 0; i < engineParent.childCount; i++)
        {
            SpawnToAnchor(engine.gameObject, engineParent.GetChild(i));
        }
    }

    public void SetWing(Wing wing)
    {
        ClearChildren(wingParent);

        for (int i = 0; i < wingParent.childCount; i++)
        {
            SpawnToAnchor(wing.gameObject, wingParent.GetChild(i));
        }
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
        foreach(Transform anchor in parent) {
            for (int n = anchor.childCount - 1; n >= 0; n--)
            {
                Destroy(anchor.GetChild(n).gameObject);
            }
        }
    }
}
