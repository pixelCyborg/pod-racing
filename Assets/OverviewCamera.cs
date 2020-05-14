using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OverviewCamera : MonoBehaviour
{
    public float transitionTime = 0.33f;
    private Vector3 origPos;
    private Vector3 origRot;
    public static OverviewCamera instance;


    private void Start()
    {
        origPos = transform.position;
        origRot = transform.rotation.eulerAngles;
        instance = this;
    }

    public void Focus(Vector3 position)
    {
        Vector3 target = position;
        target.y += 5;
        target.z -= 8;

        Vector3 lookRotation = Quaternion.LookRotation(position - target).eulerAngles;

        transform.DOMove(target, transitionTime);
        transform.DORotate(lookRotation, transitionTime);
    }

    public void UnFocus()
    {
        transform.DOMove(origPos, transitionTime);
        transform.DORotate(origRot, transitionTime);
    }
}
