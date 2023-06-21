using System;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public Transform target;

    public Vector3 Offset;

    private Camera Camera;

    void Start()
    {
        Camera = GetComponentInParent<Camera>();
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            Camera.transform.position = target.transform.position + Offset;
        }
        
        Camera.transform.LookAt(target);
    }
}