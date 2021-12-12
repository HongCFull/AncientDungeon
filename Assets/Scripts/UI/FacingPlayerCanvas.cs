using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacingPlayerCanvas : MonoBehaviour
{
    private Transform playerCameraTransform;
    private Transform transform;

    private void Start()
    {
        playerCameraTransform = PlayerMainCamera.Instance.gameObject.transform;
        transform = gameObject.transform;
    }

    void Update()
    {
        transform.LookAt(playerCameraTransform.position);
    }
}
