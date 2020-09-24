using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Garage : MonoBehaviour
{
    public static VehicleData data;
    public static PartsDB.Part[] OwnedParts
    {
        get
        {
            if (ownedParts == null) ownedParts = SaveLoadSystem.OwnedParts();
            return ownedParts;
        }

        set
        {
            ownedParts = value;
        }
    }
    private static PartsDB.Part[] ownedParts;

    public static Garage instance;

    public Transform podium;
    Chassis car;
    float speed = 15f;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SaveLoadSystem.ClearSave();
        SceneManager.sceneUnloaded += ExitGarage;
        if (data == null || OwnedParts == null) LoadVehicleData();
        SpawnVehicle();
    }

    private void ExitGarage(Scene scene)
    {
        if(scene.name == "Garage")
        {
            SaveVehicleData();
            SceneManager.sceneUnloaded -= ExitGarage;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(podium != null) podium.Rotate(new Vector3(0, 0, speed * Time.deltaTime));        
    }

    private void SpawnVehicle()
    {
        GameObject go = PartsCollection.Instance.GetPartFromRef(data.chassis).prefab;
        SpawnChassis(go.GetComponent<Chassis>());
    }

    private void SpawnChassis(Chassis chassis)
    {
        if (car != null) Destroy(car.gameObject);

        GameObject go = Instantiate(chassis.gameObject);
        car = go.GetComponent<Chassis>();
        go.transform.SetParent(podium);
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;

        SetEngine(GetEngine().GetComponent<Engine>());
        SetWing(GetWing().GetComponent<Wing>());
    }

    public static GameObject GetWing()
    {
        if (data.wing == null) return null;
        return PartsCollection.Instance.GetPartFromRef(data.wing).prefab;
    }

    public static GameObject GetEngine()
    {
        if (data.engine == null) return null;
        return PartsCollection.Instance.GetPartFromRef(data.engine).prefab;
    }

    public PartsDB.Part[] GetParts(PartsDB.PartType type)
    {
        List<PartsDB.Part> parts = new List<PartsDB.Part>();
        for(int i = 0; i < OwnedParts.Length; i++)
        {
            switch(type)
            {
                case PartsDB.PartType.Chassis:
                    if (OwnedParts[i].prefab.GetComponent<Chassis>()) parts.Add(OwnedParts[i]);
                    break;
                case PartsDB.PartType.Engine:
                    if (OwnedParts[i].prefab.GetComponent<Engine>()) parts.Add(OwnedParts[i]);
                    break;
                case PartsDB.PartType.Wing:
                    if (OwnedParts[i].prefab.GetComponent<Wing>()) parts.Add(OwnedParts[i]);
                    break;
            }
        }

        return parts.ToArray();
    }

    public static void AddPart(PartsDB.Part part)
    {
        List<PartsDB.Part> partsList = new List<PartsDB.Part>(OwnedParts);
        partsList.Add(part);
        Debug.Log("Adding: " + part.prefab.name);
        OwnedParts = partsList.ToArray();
        SaveVehicleData();
    }

    //Data setting

    public void SetChassis(Chassis chassis)
    {
        data.chassis = PartsCollection.Instance.GetPartRef(chassis.gameObject);
        data.weight = chassis.weight;
        data.handling = chassis.handling;
        data.acceleration = chassis.acceleration;
        data.velocityDrag = chassis.velocityDrag;

        StatsDisplay.instance.UpdateAcceleration(chassis.acceleration);
        StatsDisplay.instance.UpdateMaxSpeed(chassis.acceleration, chassis.velocityDrag);


        SpawnChassis(chassis);
    }

    public void SetEngine(Engine engine)
    {
        data.engine = PartsCollection.Instance.GetPartRef(engine.gameObject);
        data.boostCapacity = engine.boostCapacity;
        data.boostCost = engine.boostCost;
        data.boostFactor = engine.boostFactor;
        data.boostRegen = engine.boostRegen;

        StatsDisplay.instance.UpdateBoostEfficiency(engine.boostCapacity, engine.boostCost);
        StatsDisplay.instance.UpdateBoostPower(engine.boostFactor);
        StatsDisplay.instance.UpdateBoostRegen(engine.boostRegen);

        car.SetEngine(engine);
    }

    public void SetWing(Wing wing)
    {
        data.wing = PartsCollection.Instance.GetPartRef(wing.gameObject);
        data.turnSpeed = wing.turnSpeed;
        data.maxTurn = wing.maxTurn;
        data.turnDrag = wing.turnDrag;

        StatsDisplay.instance.UpdateTurnspeed(wing.maxTurn);
        StatsDisplay.instance.UpdateControl(wing.turnDrag);

        car.SetWing(wing);
    }

    public static void SaveVehicleData()
    {
        SaveLoadSystem.Save();
    }

    public static void LoadVehicleData()
    {
        SaveLoadSystem.Load();
        data = SaveLoadSystem.PlayerVehicle();
        OwnedParts = SaveLoadSystem.OwnedParts();
    }

    public void DeleteVehicleData()
    {
        SaveLoadSystem.ClearSave();
    }

    private void OnApplicationPause(bool pause)
    {
        if(pause) SaveVehicleData();
    }

    private void OnApplicationQuit()
    {
        SaveVehicleData();
    }
}
