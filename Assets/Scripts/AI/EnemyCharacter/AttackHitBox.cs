using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

using Combat;

[RequireComponent(typeof(Collider))]
public class AttackHitBox : MonoBehaviour
{
    [SerializeField] private List<DamageableTag> canDamageObjectsWithTag;
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
        Damageable damageableComp = other.gameObject.GetComponent<Damageable>();
        if (!damageableComp) 
            return;

        DamageableTag targetTag = damageableComp.GetDamageableTag();
        Debug.Log(targetTag);
        foreach (DamageableTag tag in canDamageObjectsWithTag) {
            if (targetTag == tag) {
                damageableComp.TakeDamageBy(damage);
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
