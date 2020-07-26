﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OverviewCamera : MonoBehaviour
{
    public float transitionTime = 0.33f;
    public Vector3 planetOffset;
    private Vector3 origPos;
    private Vector3 origRot;
    public static OverviewCamera instance;


    private void Start()
    {
        origPos = transform.position;
        origRot = transform.rotation.eulerAngles;
        instance = this;
    }

    void FitScreen(SphereCollider fitObject, float fitMultiplier = 2.0f)
    {
        float radius = fitObject.radius;
        //float distance = Mathf.Max(xyz.x, xyz.y, xyz.z);
        radius /= (2.0f * Mathf.Tan(0.5f * Camera.main.fieldOfView * Mathf.Deg2Rad));
        // Move camera in -z-direction; change '2.0f' to your needs
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -radius * fitMultiplier);
        fitObject.gameObject.SetActive(false);
    }

    public void Focus(Vector3 position, SphereCollider focus)
    {
        Vector3 target = position + planetOffset;

        Vector3 lookRotation = Quaternion.LookRotation(position - target).eulerAngles;

        transform.DOMove(target, transitionTime).OnComplete(() =>
        {
            FitScreen(focus);
        });
        transform.DORotate(lookRotation, transitionTime);
    }

    public void UnFocus()
    {
        transform.DOMove(origPos, transitionTime);
        transform.DORotate(origRot, transitionTime);
    }
}
