using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Garage : MonoBehaviour
{
    public static VehicleData data;
    public Transform podium;
    Chassis car;
    float speed = 15f;

    private void Start()
    {
        SceneManager.sceneUnloaded += ExitGarage;
        if (data == null) LoadVehicleData();
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
        GameObject go = Resources.Load(data.chassis) as GameObject;
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
        car.AttachParts();
    }

    public static GameObject GetWing()
    {
        if (string.IsNullOrEmpty(data.wing)) return null;
        return Resources.Load(data.wing) as GameObject;
    }

    public static GameObject GetEngine()
    {
        if (string.IsNullOrEmpty(data.engine)) return null;
        return Resources.Load(data.engine) as GameObject;
    }

    //Data setting

    public void SetChassis(Chassis chassis)
    {
        data.chassis = chassis.path;
        data.weight = chassis.weight;
        data.handling = chassis.handling;
        SpawnChassis(chassis);
    }

    public void SetEngine(Engine engine)
    {
        data.engine = engine.path;
        data.acceleration = engine.acceleration;
        data.velocityDrag = engine.velocityDrag;
        car.SetEngine(engine);
    }

    public void SetWing(Wing wing)
    {
        data.wing = wing.path;
        data.turnSpeed = wing.turnSpeed;
        data.maxTurn = wing.maxTurn;
        data.turnDrag = wing.turnDrag;
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
