using UnityEngine;

public class ProceduralBezierCurve : MonoBehaviour
{

    // visualization variables
    public float start = 0f;
    public float length = 5f;
    public float speed = 0.01f;

    private int randomOffset = 0;

    void Update()
    {

        start += speed;

        clampValues();
    }

    private void clampValues()
    {
        if (start > 1.0f)
        {
            int floorInt = Mathf.FloorToInt(start);
            float floorFloat = Mathf.Floor(start);
            randomOffset += floorInt * 2;
            start -= floorFloat;
        }
    }

    //
    // Determines the point on our procedural curve given it's distance on the line.
    // This is all you need to use the curve in your code.
    Vector3 CalculateProceduralBezierPoint(float i)
    {

        var prevInt = Mathf.FloorToInt(i);
        var nextInt = Mathf.CeilToInt(i);
        var t = i - prevInt;

        int prevEndPoint = prevInt * 2;
        int nextEndPoint = nextInt * 2;

        int prevControl = prevEndPoint - 1;
        int nextControl = nextEndPoint - 1;

        // find the control points
        Vector3 p0 = CalculateProceduralControlPoint(prevEndPoint);
        Vector3 p1 = p0 - (CalculateProceduralControlPoint(prevControl) - p0);
        Vector3 p2 = CalculateProceduralControlPoint(nextControl);
        Vector3 p3 = CalculateProceduralControlPoint(nextEndPoint);

        return CalculateBezierPoint(t, p0, p1, p2, p3);
    }

    //
    // Procedurally generates a control point given the control point's index
    Vector3 CalculateProceduralControlPoint(int controlPointNumber)
    {

        Random.seed = controlPointNumber + randomOffset;
        var val1 = (Random.value * 2) - 1;
        var val2 = Random.value - 0.5f;

        return new Vector3(val1, val2, controlPointNumber / 2.0f);
    }

    //
    // Calculates a point on a bezier curve given a t value, and 4 control points
    Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {

        /*
            Implementation taken from: http://devmag.org.za/2011/04/05/bzier-curves-a-tutorial/
            Written by Herman Tulleken
        */

        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0; // first term
        p += 3 * uu * t * p1; // second term
        p += 3 * u * tt * p2; // third term
        p += ttt * p3;        // fourth term

        return p;
    }

    // Visualizes the curve
    void OnDrawGizmos()
    {

        // prevents crashes in editor at high start values
        clampValues();

        // visualize the bezier curve
        float endPoint = start + length;
        var drawPosition = transform.position + new Vector3(0, 0, -(length / 2.0f) - start);
        var lastPoint = CalculateProceduralBezierPoint(start);
        for (float i = start; i <= endPoint; i += 0.0025f)
        {
            var nextPoint = CalculateProceduralBezierPoint(i);
            Gizmos.DrawLine(drawPosition + lastPoint, drawPosition + nextPoint);
            lastPoint = nextPoint;
        }
    }
}