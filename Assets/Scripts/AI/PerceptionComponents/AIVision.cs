using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AIVision : MonoBehaviour
{
    [SerializeField] private Transform visionFrom;
    [SerializeField] private LayerMask occlusionLayer;
    [SerializeField] private float occlusionScanPeriod;
    
    private RaycastHit hit;
    private float timer =0f;
    public bool canSeePlayer { get; private set; } = false;


    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.tag.Equals("Player"))
            return;
        
        if (VisionIsOccluded())
            canSeePlayer = false;
        else
            canSeePlayer = true;
        
        //Debug.Log("Enter ai vision");
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.gameObject.tag.Equals("Player"))
            return;
        
        timer += Time.deltaTime;
        if (timer >= occlusionScanPeriod) {
            timer = 0f;
            
            if (VisionIsOccluded())
                canSeePlayer = false;
            else
                canSeePlayer = true;
        }
        
        // Debug.Log("stay inside");
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.tag.Equals("Player"))
            return;
    
        if (VisionIsOccluded())
            canSeePlayer = false;
        else
            canSeePlayer = true;
    
        // Debug.Log("exit ai vision");

    }

    bool VisionIsOccluded()
    {
        Vector3 distanceVec = PlayerCharacter.Instance.GetPlayerWorldPosition() - visionFrom.position;
        return Physics.Raycast(visionFrom.position, distanceVec.normalized,out hit, distanceVec.magnitude,occlusionLayer);
    }
}
