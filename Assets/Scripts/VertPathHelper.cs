using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

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

    public TrackInfo GetNextPointDirection(Transform origin)
    {
        TrackInfo info = new TrackInfo();
        float turn = 0f;

        //The next point
        //float closestPathTime = path.GetClosestTimeOnPath(origin.position + origin.forward * predictionDistance);
        float closestPathTime = path.GetClosestTimeOnPath(origin.position);

        Vector3 position = path.GetPointAtTime(closestPathTime + pathTimePrediction, EndOfPathInstruction.Loop);
        Vector3 directionToPath = position - origin.position;
        float distanceTurn = Vector3.SignedAngle(origin.forward, directionToPath, origin.up);
        float offset = Vector3.Distance(origin.position + origin.forward * predictionDistance, position);

        //add some stupid juice
        distanceTurn += Random.Range(-25f, 25f);
        turn += distanceTurn;

        turn *= 0.1f;
        if (turn > 1.0f) turn = 1.0f;
        if (turn < 1.0f) turn = -1.0f;

        info.turn = turn;
        info.pathTime = closestPathTime;
        return info;
    }

    public float GetPathTime(Transform origin)
    {
        return path.GetClosestTimeOnPath(origin.position);
    }
}
