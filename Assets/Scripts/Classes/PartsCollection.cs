using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PartsCollection", menuName = "ScriptableObjects/PartsCollection", order = 0)]
public class PartsCollection : ScriptableObject
{
    static PartsCollection _instance;
    public static PartsCollection Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = Resources.Load<PartsCollection>("PartsCollection");
            }
            return _instance;
        }
    }

    [System.Serializable]
    public class PartRef
    {
        public PartsDB.PartType type;
        public int index;

        public PartRef(PartsDB.PartType _type, int _index)
        {
            type = _type;
            index = _index;
        }
    }

    public PartsDB chassis;
    public PartsDB engines;
    public PartsDB boosters;
    public PartsDB wings;

    public PartRef[] GetPartRefs(PartsDB.Part[] parts)
    {
        PartRef[] refs = new PartRef[parts.Length];

        for(int i = 0; i < parts.Length; i++)
        {
            refs[i] = GetPartRef(parts[i]);
        }

        return refs;
    }

    public PartRef GetPartRef(PartsDB.Part part)
    {
        PartRef partRef = new PartRef(PartsDB.PartType.Chassis, 0);

        if(part.prefab.GetComponent<Chassis>())
        {
            for(int i = 0; i < chassis.parts.Count; i++)
            {
                if(GameObject.Equals(chassis.parts[i].prefab, part.prefab))
                {
                    return new PartRef(PartsDB.PartType.Chassis, i);
                }
            }
        }
        else if(part.prefab.GetComponent<Booster>())
        {
            for (int i = 0; i < boosters.parts.Count; i++)
            {
                if (GameObject.Equals(boosters.parts[i].prefab, part.prefab))
                {
                    return new PartRef(PartsDB.PartType.Booster, i);
                }
            }
        }
        else if(part.prefab.GetComponent<Engine>())
        {
            for (int i = 0; i < engines.parts.Count; i++)
            {
                if (GameObject.Equals(engines.parts[i].prefab, part.prefab))
                {
                    return new PartRef(PartsDB.PartType.Engine, i);
                }
            }
        }
        else if (part.prefab.GetComponent<Wing>())
        {
            for (int i = 0; i < wings.parts.Count; i++)
            {
                if (GameObject.Equals(wings.parts[i].prefab, part.prefab))
                {
                    return new PartRef(PartsDB.PartType.Wing, i);
                }
            }
        }

        return partRef;
    }

    public PartRef GetPartRef(GameObject go)
    {
        PartRef partRef = new PartRef(PartsDB.PartType.Chassis, 0);

        if (go.GetComponent<Chassis>())
        {
            for (int i = 0; i < chassis.parts.Count; i++)
            {
                if (GameObject.Equals(chassis.parts[i].prefab, go))
                {
                    return new PartRef(PartsDB.PartType.Chassis, i);
                }
            }
        }
        else if (go.GetComponent<Booster>())
        {
            for (int i = 0; i < boosters.parts.Count; i++)
            {
                if (GameObject.Equals(boosters.parts[i].prefab, go))
                {
                    return new PartRef(PartsDB.PartType.Booster, i);
                }
            }
        }
        else if (go.GetComponent<Engine>())
        {
            for (int i = 0; i < engines.parts.Count; i++)
            {
                if (GameObject.Equals(engines.parts[i].prefab, go))
                {
                    return new PartRef(PartsDB.PartType.Engine, i);
                }
            }
        }
        else if (go.GetComponent<Wing>())
        {
            for (int i = 0; i < wings.parts.Count; i++)
            {
                if (GameObject.Equals(wings.parts[i].prefab, go))
                {
                    return new PartRef(PartsDB.PartType.Wing, i);
                }
            }
        }

        return partRef;
    }

    public PartsDB.Part GetPartFromRef(PartRef partRef)
    {
        switch(partRef.type)
        {
            case PartsDB.PartType.Chassis:
                return chassis.parts[partRef.index];
            case PartsDB.PartType.Engine:
                return engines.parts[partRef.index];
            case PartsDB.PartType.Booster:
                return boosters.parts[partRef.index];
            case PartsDB.PartType.Wing:
                return wings.parts[partRef.index];
        }

        return null;
    }

    public PartRef DefaultChassis()
    {
        return new PartsCollection.PartRef(PartsDB.PartType.Chassis, 0);
    }

    public PartRef DefaultEngine()
    {
        return new PartsCollection.PartRef(PartsDB.PartType.Engine, 0);
    }

    public PartRef DefaultWings()
    {
        return new PartsCollection.PartRef(PartsDB.PartType.Wing, 0);
    }

    public PartRef DefaultBooster()
    {
        return new PartsCollection.PartRef(PartsDB.PartType.Booster, 0);
    }

    public PartsDB.Part[] GetParts(PartsDB.PartType type)
    {
        switch(type)
        {
            case PartsDB.PartType.Chassis:
                return chassis.parts.ToArray();
                break;
            case PartsDB.PartType.Engine:
                return engines.parts.ToArray();
                break;
            case PartsDB.PartType.Wing:
                return wings.parts.ToArray();
                break;
            case PartsDB.PartType.Booster:
                return boosters.parts.ToArray();
                break;
        }

        return null;
    }

    public PartsDB.Part GetPart(GameObject _part)
    {
        for (int i = 0; i < chassis.parts.Count; i++)
        {
            if (GameObject.Equals(_part, chassis.parts[i].prefab)) return chassis.parts[i];
        }

        for (int i = 0; i < engines.parts.Count; i++)
        {
            if (GameObject.Equals(_part, engines.parts[i].prefab)) return engines.parts[i];
        }

        for (int i = 0; i < wings.parts.Count; i++)
        {
            if (GameObject.Equals(_part, wings.parts[i].prefab)) return wings.parts[i];
        }

        for (int i = 0; i < boosters.parts.Count; i++)
        {
            if (GameObject.Equals(_part, boosters.parts[i].prefab)) return boosters.parts[i];
        }

        return null;
    }
}
