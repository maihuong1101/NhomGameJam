using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    public Vector3 offset;
    public float speed;
    public float yorigin;
    private void Awake()
    {
        yorigin = transform.position.y;
    }
    private void LateUpdate()
    {
        Vector3 newpos = target.transform.position + offset;
        transform.position = new Vector3(newpos.x, yorigin,-10 );
    }
}
