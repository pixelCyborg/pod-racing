using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PartsDB", menuName = "ScriptableObjects/PartsDatabase", order = 1)]
public class PartsDB : ScriptableObject
{
    public enum PartType
    {
        Chassis, Engine, Booster, Wing
    }
    [System.Serializable]
    public class Part
    {
        public GameObject prefab;
    }

    public PartType databaseType;
    public List<Part> parts;
}
