using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{

    [SerializeField] private Transform hand;
    private Transform bubble;
    LineRenderer lineRenderer;
    public static Rope instance;
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        instance = this;
    }
    void Update()
    {
        if(bubble == null)
        {
            return;
        }
        lineRenderer.SetPosition(0, hand.position);
        lineRenderer.SetPosition(1, bubble.position);
    }
    public void SetBubble(Transform transform)
    {
        bubble = transform;
    }
    public void TurnOffLine()
    {
        lineRenderer.enabled = false;
    }
    public void TurnOnLine()
    {
        lineRenderer.enabled = true;
    }
}
