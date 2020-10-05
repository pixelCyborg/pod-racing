using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Overworld : MonoBehaviour
{
    public static Overworld instance;
    public Vector3 target;
    public UnityAction<Vector3> Move;
    LayerMask mask;
    public GameObject locationPrefab;

    public GameObject linePrefab;

    public OverworldPlanet[] planets;
    [HideInInspector]
    public int currentPlanetIndex = -1;

    public float lineWidth = 0.33f;

    //Solar map
    public GameObject orbitParticles;
    private List<LineRenderer> lines;
    //========

    // Start is called before the first frame update
    void Awake()
    {
        lines = new List<LineRenderer>();
        mask = 1 << LayerMask.NameToLayer("Road");
        instance = this;
    }

    private void Start()
    {
        AssignPlanetIndex(); ;
        DrawMap();
        PlacePlayer();
    }

    public void AssignPlanetIndex()
    {
        for(int i = 0; i < planets.Length; i++)
        {
            planets[i].planetIndex = i;
        }
    }

    public void DrawMap()
    {
        for(int i = lines.Count - 1; i >= 0; i--)
        {
            Destroy(lines[i].gameObject);
        }

        lines = new List<LineRenderer>();
        for(int i = 0; i < planets.Length - 1; i++)
        {
            ConnectLocations(planets[i].transform.position, planets[i + 1].transform.position);
        }
    }

    /*
    private void Update()
    {
        if (OverviewCamera.instance.zoomed) return;
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f, mask))
            {
                target = hit.point;
                if(Move != null) Move.Invoke(hit.point);
            }
        }
    }*/

    private void PlacePlayer()
    {
        if (currentPlanetIndex < 0) currentPlanetIndex = SaveLoadSystem.CurrentPlanetIndex();
        if (currentPlanetIndex < 0) currentPlanetIndex = 0;
        planets[currentPlanetIndex].Land();
    }

    public void ConnectLocations(Vector3 pointA, Vector3 pointB)
    {
        List<Vector3> points = new List<Vector3>();
        int positionCount = 24;


        points.Add(pointA);
        points.Add(pointB);
        /*
        for (int i = 0; i < positionCount; i++)
        {
            float t = (float)i / (float)positionCount;
            points.Add(Vector3.Slerp(pointA, pointB, t) * transform.localScale.x * 1.02f + transform.position);
        }
        points.Add(pointB * transform.localScale.x * 1.02f + transform.position);*/

        LineRenderer lineRend = SpawnLineRend();
        lines.Add(lineRend);
        lineRend.positionCount = points.Count;
        lineRend.widthMultiplier = lineWidth;
        lineRend.SetPositions(points.ToArray());

    }

    private LineRenderer SpawnLineRend()
    {
        GameObject go = Instantiate(linePrefab);
        return go.GetComponent<LineRenderer>();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause) SaveLoadSystem.Save();
    }

    private void OnApplicationQuit()
    {
        SaveLoadSystem.Save();
    }

    public void ShowSolarMap()
    {
        OverworldIcon.instance.Show(planets[currentPlanetIndex]);
        for(int i = 0; i < lines.Count; i++)
        {
            lines[i].enabled = true;
        }
        orbitParticles.SetActive(true);
    }

    public void HideSolarMap()
    {
        OverworldIcon.instance.Hide();
        for(int i = 0; i < lines.Count; i++)
        {
            lines[i].enabled = false;
        }
        orbitParticles.SetActive(false);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Overworld))]
public class OverworldEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Overworld overworld = (Overworld)target;
        if (GUILayout.Button("Draw Map"))
        {
            overworld.DrawMap();
        }
    }
}
#endif
