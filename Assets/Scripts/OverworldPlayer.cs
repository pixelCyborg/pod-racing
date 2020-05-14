using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OverworldPlayer : MonoBehaviour
{
    private float currentSpeed;
    public float speed = 5f;
    private float moving;

    private void Start()
    {
        Overworld.instance.Move += Move;
    }

    // Update is called once per frame
    void Update()
    {
        return;

        if(Overworld.instance != null)
        {
            if(Overworld.instance.target != Vector3.zero)
            {
                if (Vector3.Distance(Overworld.instance.target, transform.position) > 0.5f)
                {
                    Vector3 lookAt = Quaternion.LookRotation(Overworld.instance.target - transform.position).eulerAngles;
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(lookAt), Time.deltaTime * speed * 0.5f);

                    transform.position = transform.position + transform.forward * Time.deltaTime * speed;
                }
            }
        }
    }

    void Move(Vector3 move)
    {
        transform.DOKill();
        transform.DOLookAt(move, 0.2f, AxisConstraint.Y, transform.up).OnComplete(() =>
        {
            transform.DOMove(move, Vector3.Distance(transform.position, move) * 0.25f);
        });
    }
}
