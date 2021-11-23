using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Combat;
using UnityEngine;

using HitBoxDefinition;
using Unity.Mathematics;

[RequireComponent(typeof(Collider))]
public class AttackHitBox : MonoBehaviour
{
    [SerializeField] private CombatCharacter owner;
    [SerializeField] private List<HitBoxTag> canDamageHitBoxWithTag;
    [SerializeField] private ParticleSystem hitVFX;
    
    [Tooltip("This can be updated by the animation event")]
    [SerializeField] private float skillPower;
    
    private Collider attackCollider;
    private float originalDamage;

    private void Awake()
    {
        attackCollider = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision other)
    {
        ReceiveHitBox receiveHitBoxComp = other.gameObject.GetComponent<ReceiveHitBox>();
        if (!receiveHitBoxComp)
        {
           // Debug.Log("No receiver and return");
            return;
        }

        HitBoxTag targetTag = receiveHitBoxComp.GetHitBoxTag();
        foreach (HitBoxTag tag in canDamageHitBoxWithTag) {
            if (targetTag == tag) {
                CombatDamageManager.DealDamageTo(owner,receiveHitBoxComp.GetCombatCharacterOwner(),skillPower);
                ShowHitVFX(other.GetContact(0));
            }
        }
    }


    public void EnableAttackCollider()
    {
        attackCollider.enabled = true;
    }
    
    public void EnableAttackColliderWithSkillPower(float skillPower)
    {
        attackCollider.enabled = true;
        this.skillPower = skillPower;
    }
    
    /// <summary>
    /// Disable the attack collider attached with this gameObject.
    /// Force restoring back the original damage set in the inspector mode 
    /// </summary>
    public void DisableAttackCollider()
    {
        attackCollider.enabled = false;
        skillPower = originalDamage;
    }

    private void ShowHitVFX(ContactPoint contactPoint)
    {
        if (!hitVFX)
            return;

        Vector3 position = contactPoint.point;
        Quaternion rotation = Quaternion.FromToRotation(position, position + contactPoint.normal);
        Instantiate(hitVFX, position, rotation);
    }

    // private void OnTriggerEnter(Collider other)
    // {
    //     Debug.Log("trigger");
    //
    //     ReceiveHitBox receiveHitBoxComp = other.gameObject.GetComponent<ReceiveHitBox>();
    //     if (!receiveHitBoxComp)
    //     {
    //         return;
    //     }
    //
    //     HitBoxTag targetTag = receiveHitBoxComp.GetHitBoxTag();
    //     foreach (HitBoxTag tag in canDamageHitBoxWithTag) {
    //         if (targetTag == tag) {
    //             CombatDamageManager.DealDamageTo(owner,receiveHitBoxComp.GetCombatCharacterOwner(),skillPower);
    //             RaycastHit hit;
    //             if (Physics.Raycast(transform.position, transform.forward, out hit)) {
    //                 ShowHitVFX(hit.point);
    //             }
    //         }
    //     }
    // }
}
