using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

[ExecuteInEditMode]
public class VertPathHelper : MonoBehaviour
{
    public struct TrackInfo
    {
        public float pathTime;
        public float turn;
    }

    VertexPath path;
    public static VertPathHelper instance;

    public float predictionDistance = 5.0f;
    private float pathTimePrediction;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        path = GetComponent<PathCreator>().path;
        pathTimePrediction = (1.0f / path.length) * 2f;
    }

    public Quaternion GetCurrentPointRotation(Transform origin)
    {
        Vector3 euler = Vector3.zero;
        float closestPathTime = GetPathTime(origin);
        return path.GetRotation(closestPathTime, EndOfPathInstruction.Loop);
    }

    public TrackInfo GetNextPointDirection(Transform origin)
    {
        TrackInfo info = new TrackInfo();
        float turn = 0f;

        //The next point
        float closestPathTime = GetPathTime(origin);

        Vector3 position = path.GetPointAtTime(closestPathTime + pathTimePrediction, EndOfPathInstruction.Loop);
        Vector3 directionToPath = position - origin.position;
        float distanceTurn = Vector3.SignedAngle(origin.forward, directionToPath, origin.up);
        float offset = Vector3.Distance(origin.position + origin.forward * predictionDistance, position);

        //add some stupid juice
        distanceTurn += Random.Range(-5f, 5f);
        turn += distanceTurn;

        turn *= 0.1f;
        if (turn > 1.0f) turn = 1.0f;
        if (turn < -1.0f) turn = -1.0f;

        info.turn = turn;
        info.pathTime = closestPathTime;
        return info;
    }

    public float GetPathTime(Transform origin)
    {
        return path.GetClosestTimeOnPath(origin.position);
    }
}
