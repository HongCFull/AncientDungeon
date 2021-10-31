using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera Instance { get; private set; }
    
    private void Awake()
    {
        if (!Instance)
            Instance = this;
    }

    public Vector3 GetUnitForwardVectorInXZPlane()
    {
        Vector3 forwardVec3D = transform.forward;
        forwardVec3D.y = 0;
        return forwardVec3D.normalized;
    } 
}
