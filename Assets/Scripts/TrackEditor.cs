using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class TrackEditor : MonoBehaviour
{
    public GameObject previewMesh;
    public Vector3 rotationOffset = Vector3.zero;

    public GameObject boostPadPrefab;
    [HideInInspector]
    public Transform boostpadRoot;
    public bool placeBoostpads;
    public Vector3 lastRaycastHit;

    private string boostPadId = "BoostPadRoot";

    bool IsMouseOverGameWindow
    {
        get
        {
            return !(0 > Input.mousePosition.x
                || 0 > Input.mousePosition.y
                || Screen.width < Input.mousePosition.x
                || Screen.height < Input.mousePosition.y);
        }
    }

    private void Update()
    {
        if(boostpadRoot == null)
        {
            boostpadRoot = GameObject.Find(boostPadId)?.transform;
            if(boostpadRoot == null)
            {
                boostpadRoot = new GameObject(boostPadId).transform;
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 24;
        style.normal.textColor = Color.cyan;
        if(placeBoostpads) Handles.Label(ScreenToWorld(10, 10), "Placing Boostpads", style);

        if (lastRaycastHit != Vector3.zero && false)
        {
            Gizmos.DrawSphere(lastRaycastHit, 50f);
        }
    }
#endif

    Vector3 ScreenToWorld(float x, float y)
    {
        Camera camera = Camera.current;
        Vector3 s = camera.WorldToScreenPoint(transform.position);
        return camera.ScreenToWorldPoint(new Vector3(x, camera.pixelHeight - y, s.z));
    }
    Rect ScreenRect(int x, int y, int w, int h)
    {
        Vector3 tl = ScreenToWorld(x, y);
        Vector3 br = ScreenToWorld(x + w, y + h);
        return new Rect(tl.x, tl.y, br.x - tl.x, br.y - tl.y);
    }

    public int RoadMask()
    {
        return 1 << LayerMask.NameToLayer("Road");
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(TrackEditor))]
public class TrackEditorUI : Editor
{
    Vector3 lastRaycastPos;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TrackEditor trackEditor = (TrackEditor)target;
        if (GUILayout.Button("Place Boostpads"))
        {
            Debug.Log("Placing boosters");
            trackEditor.placeBoostpads = true;
        }

    }

    public void OnSceneGUI()
    {
        TrackEditor trackEditor = (TrackEditor)target;
        if (trackEditor.placeBoostpads)
        {
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
            Event e = Event.current;

            RaycastHit hit;
            if (Physics.Raycast(HandleUtility.GUIPointToWorldRay(Event.current.mousePosition), out hit, 99999f, trackEditor.RoadMask()))
            {

                if (trackEditor.previewMesh != null)
                {
                    if (VertPathHelper.instance == null) FindObjectOfType<VertPathHelper>().Initialize();

                    Quaternion targetRot = Quaternion.FromToRotation(Vector3.up, hit.normal);// * trackEditor.boostPadPrefab.transform.rotation;
                    Vector3 pathRot = VertPathHelper.instance.GetCurrentPointRotation(trackEditor.previewMesh.transform).eulerAngles;
                    pathRot += trackEditor.rotationOffset;
                    pathRot.x = targetRot.eulerAngles.x;
                    pathRot.z = targetRot.eulerAngles.z;

                    trackEditor.previewMesh.SetActive(true);
                    trackEditor.previewMesh.transform.position = hit.point;
                    trackEditor.previewMesh.transform.position += trackEditor.previewMesh.transform.up * 0.25f;
                    trackEditor.previewMesh.transform.rotation = Quaternion.Euler(pathRot);
                }

                if (e.type == EventType.MouseDown && e.button == 0)
                {
                    GameObject go = Instantiate(trackEditor.boostPadPrefab, 
                        trackEditor.previewMesh.transform.position, 
                        trackEditor.previewMesh.transform.rotation, 
                        trackEditor.boostpadRoot) as GameObject;
                }
            }
            else
            {
                trackEditor.previewMesh.SetActive(false);
            }


            //End booster placement
            if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Escape)
            {
                Debug.Log("Stop placing boosters");
                trackEditor.placeBoostpads = false;
            }
        }
        else
        {
            trackEditor.previewMesh.SetActive(false);
        }
    }
}

#endif
