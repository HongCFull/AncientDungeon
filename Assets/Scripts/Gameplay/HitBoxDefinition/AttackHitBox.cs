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

    //BUG: When player attacks the AI, it wont damage the AI properly. As the damageable script and the hit box of the enemy aren't attached in the same gameobject. 
    private void OnTriggerEnter(Collider other)
    {
        ReceiveHitBox receiveHitBoxComp = other.gameObject.GetComponent<ReceiveHitBox>();
        if (!receiveHitBoxComp) 
            return;

        HitBoxTag targetTag = receiveHitBoxComp.GetHitBoxTag();
        foreach (HitBoxTag tag in canDamageHitBoxWithTag) {
            if (targetTag == tag) {
                CombatDamageManager.DealDamageTo(owner,receiveHitBoxComp.GetCombatCharacterOwner(),skillPower);
                //combatCharacterHitBoxComp.TakeDamageBy(skillPower);
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

    private void ShowHitVFX(Vector3 position)
    {
        if (!hitVFX)
            return;
        Instantiate(hitVFX, position, quaternion.identity);
    }
    
}
