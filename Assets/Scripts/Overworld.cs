using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Overworld : MonoBehaviour
{
    public static Overworld instance;
    public Vector3 target;
    public UnityAction<Vector3> Move;
    LayerMask mask;
    public GameObject locationPrefab;

    // Start is called before the first frame update
    void Awake()
    {
        mask = 1 << LayerMask.NameToLayer("Road");
        instance = this;
    }

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
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause) SaveLoadSystem.Save();
    }

    private void OnApplicationQuit()
    {
        SaveLoadSystem.Save();
    }
}
