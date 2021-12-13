using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacingPlayerCanvas : MonoBehaviour
{
    private Transform playerCameraTransform;
    private Transform transformComp;

    private void Start()
    {
        playerCameraTransform = PlayerMainCamera.Instance.gameObject.transform;
        transformComp = gameObject.transform;
    }

    void Update()
    {
        transformComp.LookAt(playerCameraTransform.position);
    }
}
