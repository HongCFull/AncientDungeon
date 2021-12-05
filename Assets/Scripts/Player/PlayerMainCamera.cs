using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerMainCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera playerFollowVirtualCamera;

    [Tooltip("The min camera distance of the player follow virtual camera")]
    [SerializeField] [Range(0,Mathf.Infinity)] private float minCameraDistance;
    
    [Tooltip("The max camera distance of the player follow virtual camera")]
    [SerializeField] [Range(0,Mathf.Infinity)] private float maxCameraDistance;

    [SerializeField] private float cameraDistanceOffset;
    
    /// <summary>
    /// It has to be an instance because animator state machine behavior doesn't allow scene object referencing
    /// Don't use this unless it is necessary 
    /// </summary>
    public static PlayerMainCamera Instance { get; private set; }
    
    private Cinemachine3rdPersonFollow tpsVirtualCamera;
    
    private void Awake()
    {
        if (!Instance)
            Instance = this;
        
        tpsVirtualCamera = (playerFollowVirtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Body)) as Cinemachine3rdPersonFollow;
    }

    public Vector3 GetUnitForwardVectorInXZPlane()
    {
        Vector3 forwardVec3D = transform.forward;
        forwardVec3D.y = 0;
        return forwardVec3D.normalized;
    }
    
//TODO: interpolate it smoothly 
    public void ZoomInCameraByOneLevel()
    {
        if (!tpsVirtualCamera) {
            throw new Exception("Couldn't find 3d person following camera. Check if the cinemachine virtual camera is a 3rd person following camera ");
        }
        tpsVirtualCamera.CameraDistance = Mathf.Clamp(tpsVirtualCamera.CameraDistance + cameraDistanceOffset,
            minCameraDistance, maxCameraDistance);
    }

//TODO: interpolate it smoothly
    public void ZoomOutCameraByOneLevel()
    {
        if (!tpsVirtualCamera) {
            throw new Exception("Couldn't find 3d person following camera. Check if the cinemachine virtual camera is a 3rd person following camera ");
        }
        tpsVirtualCamera.CameraDistance = Mathf.Clamp(tpsVirtualCamera.CameraDistance - cameraDistanceOffset,
            minCameraDistance, maxCameraDistance);
    }

}
