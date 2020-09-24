using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : CarComponent
{
    public const float MIN_BOOST_FACTOR = 1.1f;
    public const float MAX_BOOST_FACTOR = 1.8f;

    public const float MIN_BOOST_COST = 10;
    public const float MAX_BOOST_COST = 50;

    public const float MIN_BOOST_CAPACITY = 50f;
    public const float MAX_BOOST_CAPACITY = 300f;

    public const float MIN_BOOST_REGEN = 5;
    public const float MAX_BOOST_REGEN = 30;

    [Range(MIN_BOOST_FACTOR, MAX_BOOST_FACTOR)]
    public float boostFactor = 1.2f; //Just a multiplier of how much the boost increases our acceleration. The power of the booster
    [Range(MIN_BOOST_COST, MAX_BOOST_COST)]
    public float boostCost = 25; //How much the booster costs to keep going. This should increase along with the boostFactor
    [Range(MIN_BOOST_CAPACITY, MAX_BOOST_CAPACITY)]
    public float boostCapacity = 100; //How large our boost tank is, this should usually be 100
    [Range(MIN_BOOST_REGEN, MAX_BOOST_REGEN)]
    public float boostRegen = 15; //How fast we get our boost meter back when not boosting

    public Transform boostAnchor;

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
