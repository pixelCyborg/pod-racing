using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Chassis : CarComponent
{
    public const float MAX_WEIGHT = 1.5f;
    public const float MIN_WEIGHT = 0.5f;
    public const float MAX_HANDLING = 1.5f;
    public const float MIN_HANDLING = 0.5f;

    [Header("Stats")]
    [Range(MIN_WEIGHT, MAX_HANDLING)]
    public float weight = 1.0f; //This increases max speed but decreases acceleration
    [Range(MIN_HANDLING, MAX_HANDLING)]
    public float handling = 1.0f; //Increases turn speed, strafe speed

    [Header("Anchors")]
    public Transform engineParent;
    public Transform wingParent;
    private Engine engine;
    private Booster booster;
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

    public void SetBooster(Booster _booster)
    {
        booster = _booster;

        for(int i = 0; i < engineParent.childCount; i++)
        {
            Engine _engine = engineParent.GetChild(i).GetComponentInChildren<Engine>();
            _engine.SetBooster(_booster);
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
        newObject.gameObject.layer = gameObject.layer;
        for (int i = 0; i < newObject.childCount; i++) {
            GameObject child = newObject.GetChild(i).gameObject;
            child.layer = gameObject.layer;
        }
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
