using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class BezierPathGenerator : MonoBehaviour
{
    PathCreator pathCreator;
    List<Vector3> points;
    List<Vector3> unlistedPoints;
    BoxCollider extents;

    public Transform goParent;
    public int length = 12;

    public void GeneratePath()
    {
        pathCreator = GetComponent<PathCreator>();
        extents = GetComponent<BoxCollider>();
        points = new List<Vector3>();
        unlistedPoints = new List<Vector3>();

        for(int i = goParent.childCount - 1; i >= 0; i--)
        {
            Destroy(goParent.GetChild(i).gameObject);
        }

        for(int i = 0; i < length; i++)
        {
            Vector3 pos = new Vector3(
                Random.Range(extents.bounds.min.x, extents.bounds.max.x),
                Random.Range(extents.bounds.min.y, extents.bounds.max.y),
                Random.Range(extents.bounds.min.z, extents.bounds.max.z)
            );
            CreatePointObject(pos);
            unlistedPoints.Add(pos);
        }

        int failsafe = 0;
        while(unlistedPoints.Count > 0 || failsafe < 1000)
        {
            failsafe++;
        }


        //Clear and rebuild path
        for(int i = 0; i < pathCreator.bezierPath.NumAnchorPoints; i++)
        {
            pathCreator.bezierPath.DeleteSegment(i);
        }

        for(int i = 0; i < points.Count; i++)
        {
            pathCreator.bezierPath.AddSegmentToEnd(points[i]);   
        }
    }

    public void CreatePointObject(Vector3 pos)
    {
        GameObject go = new GameObject("PathPoint");
        go.transform.position = pos;
        go.transform.SetParent(goParent);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(BezierPathGenerator))]
public class BezierPathGeneratorEditor : Editor
{
    BezierPathGenerator generator;
    void OnEnable()
    {
        generator = target as BezierPathGenerator;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(GUILayout.Button("Generate"))
        {
            generator.GeneratePath();
        }
    }
}
#endif
