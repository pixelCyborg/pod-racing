using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RaceVehicle : MonoBehaviour
{
    //Events
    public UnityEvent OnRaceComplete;

    //Refs
    public LayerMask roadLayer;
    public float hoverDistance = 2.0f;
    public Camera followedCamera;
    private CameraShake camShake;
    private UnityEngine.Rendering.Universal.PostProcessData postProcess;
    public float speedFovDistortion = 10f;
    private float origFov;

    private Animator anim;

    [HideInInspector]
    public float currentFuel;

    //Vehicle configuration properties ======================
    [Header("Throttle")]
    [Range(0.1f, 2f)]
    public float acceleration = 0.14f;
    [Range(0.0f, 1.0f)]
    public float velocityDrag = 0.98f;

    [Header("Boost")]
    [Range(1.0f, 2.0f)]
    public float boostFactor = 1.2f;
    public float boostCapacity = 100;
    public float boostCost = 25;
    public float boostRegen = 15;

    [Header("Strafe")]
    public float strafeSpeed = 0.4f;
    public float maxStrafe = 4f;
    [Range(0.0f, 1.0f)]
    public float strafeDrag = 0.85f;

    [Header("Turning")]
    public float turnSpeed = 0.5f;
    public float maxTurn = 1.6f;
    [Range(0.0f, 1.0f)]
    public float turnDrag = 0.66f;

    [Header("Visuals")]
    public float shakeThreshold = 0.92f;
    public float shakeAmount = 0.1f;
    public float magnetizeSpeed = 3.0f;
    public ParticleSwitch engineParticles;
    public ParticleSwitch boostTrails;
    public ParticleSwitch windstreaks;
    //=====================================================

    [Header("Audio")]

    [Range(0f, 1f)]
    public float speedDetune = 2.0f;
    public AudioSource engineSource;
    public AudioSource boostSource;
    public AudioSource strafeSource;

    [Header("Mesh")]
    public Transform meshParent;

    //Input values
    float turn;
    float throttle;
    float strafe;
    bool boosting;
    bool drifting;

    //Current physics stuff
    bool customPhysicsEnabled = true;
    float velocity_throttle;
    float velocity_strafe;
    float velocity_turn;
    Vector3 driftDirection = Vector3.zero;
    Vector3 inertia = Vector3.zero;
    Vector3 currentVel;
    int everythingButRoads;
    int finishPosition;


    //Race data

    [HideInInspector]
    public bool isPlayer = false;
    private int lap = 0;
    public List<float> lapTimes;
    private float currentPathPosition = 0;
    private AnchorToEngines[] particleAnchors;
    private List<Animator> particleAnims;

    //Unity engine physics
    Rigidbody body;

    //Control Input Functions
    public void SetTurn(float _turn)
    {
        turn = _turn;
        //turn += -strafe * 0.25f;
    }

    public void SetThrottle(float _throttle)
    {
        if (customPhysicsEnabled) throttle = _throttle;
        else throttle = 0f;

        if (throttle < 0f) throttle = 0f;
    }

    public void SetStrafe(float _strafe)
    {
        if (customPhysicsEnabled) strafe = _strafe;
        else strafe = 0f;
    }

    public void SetBoost(bool _boost)
    {
        if(boosting != _boost)
        {
            if(_boost)
            {
                //currentFuel -= boostCost * 0.2f;
                boostSource.Play();
            }
            else
            {
                boostSource.Stop();
            }
        }
        boosting = _boost;
        UpdateParticleAnims("boosting", _boost);
    }

    public void SetDrift(bool _drift)
    {
        drifting = _drift;
    }

    public void LStrafe()
    {
        if (drifting) return;
        strafeSource?.Play();
        velocity_strafe = -maxStrafe;
    }

    public void RStrafe()
    {
        if (drifting) return;
        strafeSource?.Play();
        velocity_strafe = maxStrafe;
    }

    private void Awake()
    {
        particleAnchors = GetComponentsInChildren<AnchorToEngines>();
        particleAnims = new List<Animator>();
        for(int i = 0; i < particleAnchors.Length; i++)
        {
            particleAnims.AddRange(particleAnchors[i].GetComponentsInChildren<Animator>());
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentFuel = boostCapacity;
        body = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        boostTrails.SwitchOff();
        engineParticles.SwitchOff();

        if(engineSource == null) engineSource = GetComponent<AudioSource>();
        if (boostSource == null) boostSource = engineParticles.GetComponent<AudioSource>();

        if(engineSource != null)
        {
            engineSource.loop = true;
            engineSource.Play();
        }

        if(followedCamera != null)
        {
            origFov = followedCamera.fieldOfView;
            camShake = followedCamera.GetComponentInParent<CameraShake>();
        }

        finishPosition = -1;
        lapTimes = new List<float>();
        everythingButRoads =~ LayerMask.GetMask("Road");
    }

    // Update is called once per frame
    void Update()
    {
        if (throttle > 0.1f) engineParticles.SwitchOn();
        else engineParticles.SwitchOff();

        if (CurrentOutOfMaxSpeed() > 1.1f)
        {
            if (windstreaks != null)
            {
                windstreaks.SwitchOn();
            }
        }
        else
        {
            if(windstreaks) windstreaks.SwitchOff();
        }

        if(boosting)
        {
            if (currentFuel > 1)
            {
                boostTrails.SwitchOn();
                currentFuel -= boostCost * Time.deltaTime;
                if (isPlayer) SpeedLinesOverlay.On();
            }
            //Dont regen fuel until no longer boosting
            else {
                boostTrails.SwitchOff();
                if (isPlayer) SpeedLinesOverlay.Off();
            }
        }
        else if (currentFuel < boostCapacity)
        {
            boostTrails.SwitchOff();
            currentFuel += boostRegen * Time.deltaTime;
            if (currentFuel > boostCapacity) currentFuel = boostCapacity;

            if (isPlayer) SpeedLinesOverlay.Off();
        }

        if (drifting)
        {
            if(driftDirection == Vector3.zero) driftDirection = transform.forward;
            throttle = 0f;
            drifting = true;
        }
        else if (driftDirection != Vector3.zero)
        {
            driftDirection = Vector3.zero;
        }

        anim.SetFloat("Strafe", velocity_turn);

        if (followedCamera)
        {
            float amount = CurrentOutOfMaxSpeed();
            if (amount > 1.0f) amount += (amount - 1f) * 5f;
            //if (amount > 1.66f) amount = 1.66f;

            followedCamera.fieldOfView = origFov + (speedFovDistortion * amount);
            HandleCameraShake();
        }

        UpdateEngineSound();
    }

    public void UpdateUI()
    {
        BoostMeter.instance.SetBoost(currentFuel, boostCapacity);

        if (Speedometer.instance != null)
        {
            Speedometer.instance.SetSpeed(velocity_throttle, MaxSpeed());
            Speedometer.instance.SetStrafe(velocity_strafe);
        }
    }

    //Physics stuff goes here
    private void FixedUpdate()
    {
        if (RaceManager.instance.raceStarted == false) return;
        MagnetizeToGround();

        UpdateSpeed();
        UpdateStrafe();
        UpdateTurn();

        if (customPhysicsEnabled)
        {
            UpdatePosition();
        }
        UpdateVelocities();
        PreCheckCollision();
    }

    void MagnetizeToGround()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, -transform.up, out hit, (hoverDistance * 5), roadLayer))
        {
            ToggleCustomPhysics(true);

            float magnetism = Time.fixedDeltaTime * magnetizeSpeed;
            if (hit.distance < hoverDistance * 0.2f)
            {
                transform.position = hit.point + transform.up * hoverDistance* 0.2f;
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, hit.point + transform.up * hoverDistance, magnetism);
            }
        }
        else
        {
            ToggleCustomPhysics(false);
        }
    }

    void PreRotate(Vector3 newPos)
    {
        RaycastHit hit;
        if (Physics.Raycast(newPos, -transform.up, out hit, (hoverDistance * 5), roadLayer))
        {
            float magnetism = Time.fixedDeltaTime * magnetizeSpeed;
            if (hit.distance < hoverDistance * 0.2f)
            {
                transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            }
            else
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation, magnetism);
            }
        }
    }

    void ToggleCustomPhysics(bool enabled)
    {
        if (enabled && !customPhysicsEnabled)
        {
            customPhysicsEnabled = true;
            body.constraints = RigidbodyConstraints.FreezeAll;
        }
        else if (!enabled && customPhysicsEnabled)
        {
            body.constraints = RigidbodyConstraints.None;
            StartCoroutine(TransferVelocity());
            customPhysicsEnabled = false;
        }
    }

    IEnumerator TransferVelocity()
    {
        yield return null;
        body.AddForce(((drifting ? driftDirection : transform.forward) * velocity_throttle + inertia) * 10, ForceMode.VelocityChange);
    }

    void UpdatePosition()
    {
        Vector3 velocity = GetVelocity();
        currentVel = velocity;

        //Rotation
        PreRotate(transform.position + velocity);
        transform.Rotate(Vector3.up, velocity_turn, Space.Self);
    }

    Vector3 GetVelocity()
    {
        Vector3 velocity = drifting? driftDirection *velocity_throttle : transform.forward* velocity_throttle;
        velocity += transform.right * velocity_strafe;
        velocity += inertia;
        transform.position += velocity;
        return velocity;
    }

    void UpdateVelocities()
    {

        velocity_strafe *= strafeDrag;
        if(!drifting) velocity_throttle *= velocityDrag;
        velocity_turn *= turnDrag;

        if (Mathf.Abs(velocity_strafe) < 0.01f) velocity_strafe = 0f;
        if (Mathf.Abs(velocity_throttle) < 0.01f) velocity_throttle = 0f;
        if (Mathf.Abs(velocity_turn) < 0.01f) velocity_turn = 0f;

        //Inertia decays twice as fast because physics is a lie
        inertia *=  0.95f;

        //Ensure no going through the road
        Vector3 down = -transform.up;
        float dot = Vector3.Dot(inertia, down);
        if (dot > 0f) inertia -= down * dot;

        if (Vector3.Magnitude(inertia) < 0.01f) inertia = Vector3.zero;
    }

    void UpdateSpeed()
    {
        bool boostable = boosting && currentFuel > 1;
        velocity_throttle += acceleration * throttle * (boostable ? boostFactor : 1f);

        if (velocity_throttle < 0) velocity_throttle = 0;
    }

    void UpdateStrafe()
    {
        bool boostable = boosting && currentFuel > 1;
        velocity_strafe += strafeSpeed * strafe * (boostable ? 0f : 1f);
    }

    void UpdateTurn()
    {
        velocity_turn += turnSpeed * turn;
        if (velocity_turn > MaxTurn()) velocity_turn = MaxTurn();
        if (velocity_turn < -MaxTurn()) velocity_turn = -MaxTurn();
    }

    private void UpdateEngineSound()
    {
        engineSource.pitch = 1.0f+ (CurrentOutOfMaxSpeed() * speedDetune);
    }

    private void HandleCameraShake()
    {
        float shake = ShakeValue();

        if (shake > 0)
        {
            camShake.shakeDuration = 0f;
            camShake.shakeAmount = 0f;
            camShake.ShakeCamera(shake * shakeAmount, 0.25f);
        }
    }

    private float ShakeValue()
    {
        //                      Threshold
        // ======================|==== Max speed
        //                        ==== Cam Shake

        float currentOutOfMax = CurrentOutOfMaxSpeed();
        float currentAboveThreshold = currentOutOfMax - shakeThreshold;
        if (currentAboveThreshold <= 0) return 0f;

        float maxAboveThreshold = 1.0f - shakeThreshold;
        return currentAboveThreshold / maxAboveThreshold;
    }

    private float CurrentOutOfMaxSpeed()
    {
        float val = 0.0f;
        val = velocity_throttle / MaxSpeed();
        return val;
    }

    private void OnCollisionStay(Collision collision)
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (everythingButRoads != (everythingButRoads | (1 << collision.gameObject.layer))) return;

        Vector3 avgContactPoint = Vector3.zero;
        for(int i = 0; i < collision.contactCount; i++)
        {
            avgContactPoint += collision.GetContact(i).normal;
        }
        avgContactPoint = avgContactPoint / collision.contactCount;
        Collide(avgContactPoint, collision.transform.tag == "Wall");
    }

    private void PreCheckCollision()
    {
        RaycastHit hit;
        bool collisionImminent = Physics.SphereCast(transform.position, 1.0f, currentVel.normalized, out hit, currentVel.magnitude, everythingButRoads, QueryTriggerInteraction.Ignore);
        if(collisionImminent)
        {
            Collide(hit.normal, hit.transform.tag == "Wall");
        }
    }

    private void Collide(Vector3 collisionNormal, bool solid = false)
    {
        inertia = collisionNormal;
        float forceMultiplier = velocity_throttle * (solid ? 1.5f : 0.2f);
        if (forceMultiplier < 2) forceMultiplier = 2;

        float impactForce = Mathf.Abs(Vector3.Angle(transform.forward, collisionNormal) / 180);
        impactForce = MathHelpers.ConvertRange(0.5f, 1.0f, 0.1f, 1.0f, impactForce);

        inertia *= impactForce * forceMultiplier;

        Vector3 down = -transform.up;
        float dot = Vector3.Dot(inertia, down);
        if (dot > 0f) inertia -= down * dot;
    }

    private float MaxTurn()
    {
        return maxTurn;
    }

    //Brute force solution then store the value. Math is for suckers.
    private float maxSpeed = -1;
    private float MaxSpeed()
    {
        if (maxSpeed < 0)
        {
            float velocity  = 0f;
            float decelleration = velocity * velocityDrag;
            int failsafe = 0;

            while (decelleration < (acceleration - 0.000001f) && failsafe < 2000)
            {
                velocity += acceleration;
                decelleration = velocity * (1f - velocityDrag);
                velocity *= velocityDrag;
                //Debug.Log("(" + velocity + "," + decelleration + ")");

                failsafe++;
            }

            maxSpeed = velocity;
        }

        return maxSpeed;
    }

    public Vector3 CurrentVelocity()
    {
        return drifting ? driftDirection * velocity_throttle : transform.forward * velocity_throttle;
    }

    public void UpdatePathPosition(float pathPosition)
    {
        currentPathPosition = pathPosition;
    }

    public void UpdateParticleAnims(string id, object val)
    {
        for(int i = 0; i < particleAnims.Count; i++)
        {
            if(val is bool)
            {
                particleAnims[i].SetBool(id, (bool)val);
            }
            else if(val is float)
            {
                particleAnims[i].SetFloat(id, (float)val);
            }
            else if(val is int)
            {
                particleAnims[i].SetInteger(id, (int)val);
            }
        }
    }

    //Race positions
    public void Lap()
    {
        lap++;
        if (lap <= 1) return;
        lapTimes.Insert(0, CurrentLapTime());
        if (lap > RaceManager.raceData.laps && finishPosition < 0) FinishRace();
        //if (GetComponent<PlayerVehicleController>()) RaceManager.instance.Lap();
    }

    public float GetTotalRaceTime()
    {
        float totalTime = 0;
        for(int i = 0; i < lapTimes.Count; i++)
        {
            totalTime += lapTimes[i];
        }
        return totalTime;
    }

    public float CurrentLapTime()
    {
        if (lapTimes != null && lapTimes.Count > 0)
        {
            return Time.time - lapTimes[0];
        }
        else if (!RaceManager.instance.raceStarted) return 0f;
        else return ElapsedTime();
    }

    public float ElapsedTime()
    {
        return Time.time - RaceManager.instance.startTime;
    }

    public int GetCurrentLap()
    {
        if (lap < 1) return 1;
        if (lap > RaceManager.raceData.laps) return lap;
        return lap;
    }

    public int GetCurrentPosition()
    {
        int position = -1;
        List<RaceVehicle> vehicles = RaceManager.instance.racers;
        for(int i = 0; i < vehicles.Count; i++)
        {
            //If this is us
            if(GameObject.ReferenceEquals(vehicles[i].gameObject, gameObject))
            {
                position = i;
            }
        }
        return position;
    }

    public float RacePosition()
    {
        if (finishPosition > -1) return (float)lap + (99 - finishPosition); 
        return lap + currentPathPosition; 
    }

    private void FinishRace()
    {
        finishPosition = GetCurrentPosition();
        Debug.Log(name + " finished in " + finishPosition + "!");
        OnRaceComplete.Invoke();
    }

    public void LoadPlayerVehicle(string saveName = "")
    {
        if (saveName == "") saveName = "player";
        VehicleData data = SaveLoadSystem.PlayerVehicle();
        GameObject go = Resources.Load(data.chassis) as GameObject;
        SpawnChassis(go.GetComponent<Chassis>());

        for(int i = 0; i < particleAnchors.Length; i++)
        {
            particleAnchors[i].SnapToEngines(transform);
        }
    }

    private void UpdateStats()
    {

    }

    private void SpawnChassis(Chassis chassis)
    {
        ClearMesh();

        GameObject go = Instantiate(chassis.gameObject);
        go.transform.SetParent(meshParent);
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;

        UpdateWindstreakMesh(go.GetComponent<MeshRenderer>());

        go.GetComponent<Chassis>().AttachParts();
    }

    private void ClearMesh()
    {
        for(int i = meshParent.childCount - 1; i >= 0; i--)
        {
            Destroy(meshParent.GetChild(i).gameObject);
        }
    }

    private void UpdateWindstreakMesh(MeshRenderer meshRend)
    {
        ParticleSystem windParticles = windstreaks.GetComponent<ParticleSystem>();
        ParticleSystem.ShapeModule shape = windParticles.shape;
        shape.meshRenderer = meshRend;
    }

    //Environmental Stuff
    public void Boost(float strength)
    {
        velocity_throttle *= strength;
    }
}
