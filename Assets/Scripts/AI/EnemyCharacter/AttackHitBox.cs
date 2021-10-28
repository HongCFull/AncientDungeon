using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

using Combat;

[RequireComponent(typeof(Collider))]
public class AttackHitBox : MonoBehaviour
{
    [SerializeField] private List<HitBoxTag> canDamageObjectsWithTag;
    [SerializeField] private float damage;
    
    private Collider attackCollider;
    private float originalDamage;

    private void Awake()
    {
        attackCollider = GetComponent<Collider>();
    }

    //BUG: When player attacks the AI, it wont damage the AI properly. As the damageable script and the hit box of the enemy aren't attached in the same gameobject. 
    private void OnTriggerEnter(Collider other)
    {
        CombatCharacterHitBox combatCharacterHitBoxComp = other.gameObject.GetComponent<CombatCharacterHitBox>();
        if (!combatCharacterHitBoxComp) 
            return;

        HitBoxTag targetTag = combatCharacterHitBoxComp.GetSelfHitBoxTag();
        foreach (HitBoxTag tag in canDamageObjectsWithTag) {
            if (targetTag == tag) {
                combatCharacterHitBoxComp.TakeDamageBy(damage);
            }
        }
    }
    

    public void EnableAttackCollider()
    {
        attackCollider.enabled = true;
    }
    
    public void EnableAttackColliderWithDamage(float damage)
    {
        attackCollider.enabled = true;
        this.damage = damage;
    }
    
    /// <summary>
    /// Disable the attack collider attached with this gameObject.
    /// Force restoring back the original damage set in the inspector mode 
    /// </summary>
    public void DisableAttackCollider()
    {
        attackCollider.enabled = false;
        damage = originalDamage;
    }
    
    
}
