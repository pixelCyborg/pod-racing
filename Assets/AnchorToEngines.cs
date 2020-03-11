using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorToEngines : MonoBehaviour
{
    List<Transform> engineTransforms;

    private void Update()
    {
        if (engineTransforms == null) return;
        for(int i = 0; i < transform.childCount; i++)
        {
            GameObject go = transform.GetChild(i).gameObject;
            if (i < engineTransforms.Count)
            {
                go.SetActive(true);
                go.transform.position = engineTransforms[i].position;
                go.transform.rotation = engineTransforms[i].rotation;
            }
            else
            {
                go.SetActive(false);
            }
        }
    }

    public void SnapToEngines(Transform root)
    {
        Engine[] engines = root.GetComponentsInChildren<Engine>();
        engineTransforms = new List<Transform>();

        for(int i = 0; i < engines.Length; i++)
        {
            engineTransforms.Add(engines[i].transform);
        }
    }
}
