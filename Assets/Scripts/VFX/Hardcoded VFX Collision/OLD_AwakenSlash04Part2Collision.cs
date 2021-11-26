using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// As particle collision doesn't fit the way of making that VFX
/// The collision detection is hardcoded instead
/// </summary>
///

//This VFX is depreciated now 
public class OLD_AwakenSlash04Part2Collision : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private AttackHitBox slash1;
    [SerializeField] private AttackHitBox slash2;
    [SerializeField] private AttackHitBox slash3;
    [SerializeField] private AttackHitBox slash4;
    [SerializeField] private AttackHitBox slash5;

    private int operationIndex = 0;
    private void Update()
    {
        if (particleSystem == null)
            return;
        
        if (operationIndex ==0 && particleSystem.time>0.13f)
        {
            operationIndex++;
           // Debug.Log(" slash1.EnableAttackCollider");
            slash1.EnableAttackCollider();
        }
        else if (operationIndex ==1 && particleSystem.time> 0.23f)
        {            
            operationIndex++;
            //Debug.Log(" slash2.EnableAttackCollider");
            slash2.EnableAttackCollider();
        }
        else if (operationIndex ==2 && particleSystem.time> 0.26f)
        {
            operationIndex++;
           // Debug.Log(" slash1.DisableAttackCollider");
            slash1.DisableAttackCollider();
        }
        else if (operationIndex ==3 && particleSystem.time> 0.3f)
        {
            operationIndex++;
           // Debug.Log(" slash3.EnableAttackCollider");
            slash3.EnableAttackCollider();
        }
        else if (operationIndex ==4 && particleSystem.time> 0.36f)
        {
            operationIndex++;
           // Debug.Log(" slash2.DisableAttackCollider");
            slash2.DisableAttackCollider();
        }
        else if (operationIndex ==5 && particleSystem.time> 0.39f)
        {
            operationIndex++;
           // Debug.Log(" slash4.EnableAttackCollider");
            slash4.EnableAttackCollider();
        }
        else if (operationIndex ==6 && particleSystem.time> 0.43f)
        {
            operationIndex++;
           // Debug.Log(" slash3.DisableAttackCollider");
            slash3.DisableAttackCollider();
        }
        else if (operationIndex ==7 && particleSystem.time> 0.45f)
        {
            operationIndex++;
           // Debug.Log(" slash5.EnableAttackCollider");
            slash5.EnableAttackCollider();
        }
        else if (operationIndex ==8 && particleSystem.time> 0.53f)
        {
            operationIndex++;
            //Debug.Log("slash4.DisableAttackCollider");
            slash4.DisableAttackCollider();
        }
        else if (operationIndex ==9 && particleSystem.time> 0.63f)
        {
            operationIndex++;
           // Debug.Log("slash5.DisableAttackCollider");
            slash5.DisableAttackCollider();
        }



    }
}
