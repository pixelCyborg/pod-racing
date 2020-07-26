using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OverviewCamera : MonoBehaviour
{
    public float transitionTime = 0.33f;
    public Vector3 planetOffset;
    private Vector3 origPos;
    private Vector3 origRot;
    private float origZPos;
    private Transform anchor;
    public static OverviewCamera instance;


    private void Start()
    {
        anchor = transform.parent;
        origPos = anchor.position;
        origRot = anchor.rotation.eulerAngles;
        origZPos = transform.localPosition.z;
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

    public void Focus(Vector3 position, SphereCollider collider)
    {
        Vector3 target = position;
        Vector3 lookRotation = Quaternion.LookRotation(position - target).eulerAngles;

        anchor.DOMove(target, transitionTime);
        //anchor.DORotate(lookRotation, transitionTime);
        transform.DOLocalMoveZ(collider.radius * collider.transform.localScale.x * -2f, transitionTime);
    }

    public void UnFocus()
    {
        anchor.DOMove(origPos, transitionTime);
        //anchor.DORotate(origRot, transitionTime);
        transform.DOLocalMoveZ(origZPos, transitionTime);
    }
}
