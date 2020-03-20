using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Chassis : CarComponent
{
    [Header("Stats")]
    public float weight = 1.0f; //This increases max speed but decreases acceleration
    public float handling = 1.0f; //Increases turn speed, strafe speed

    public Transform engineParent;
    public Transform wingParent;

    private Engine engine;
    private Wing wing;

    public void AttachParts()
    {
        GameObject engine = Garage.GetEngine();
        GameObject wing = Garage.GetWing();

        if(engine != null) SetEngine(engine.GetComponent<Engine>());
        if(wing != null) SetWing(wing.GetComponent<Wing>());
    }

    public void SetEngine(Engine _engine)
    {
        engine = _engine;
        ClearChildren(engineParent);

        for(int i = 0; i < engineParent.childCount; i++)
        {
            SpawnToAnchor(_engine.gameObject, engineParent.GetChild(i));
        }
    }

    public void SetWing(Wing _wing)
    {
        wing = _wing;
        ClearChildren(wingParent);

        for (int i = 0; i < wingParent.childCount; i++)
        {
            SpawnToAnchor(_wing.gameObject, wingParent.GetChild(i));
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

    private void SaveSetup()
    {

    }
}
