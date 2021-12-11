using System.Collections.Generic;
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
    
    [SerializeField] Collider attackCollider;
    
    
    private void OnTriggerEnter(Collider other)
    {
        ReceiveHitBox receiveHitBoxComp = other.gameObject.GetComponent<ReceiveHitBox>();
        if (!receiveHitBoxComp) {
            return;
        }
    
        HitBoxTag targetTag = receiveHitBoxComp.GetHitBoxTag();
        foreach (HitBoxTag tag in canDamageHitBoxWithTag) {
            if (targetTag == tag) {
                CombatDamageManager.DealDamageTo(owner,receiveHitBoxComp.GetCombatCharacterOwner(),skillPower);
                ShowHitVFX(attackCollider.ClosestPointOnBounds(other.transform.position));
            }
        }
    }
    
    public void EnableAttackCollider()
    {
        if(!attackCollider)
            attackCollider = GetComponent<Collider>();
        attackCollider.enabled = true;
    }
    
    public void EnableAttackColliderWithSkillPower(float skillPower)
    {
        attackCollider.enabled = true;
        this.skillPower = skillPower;
    }
    
    /// <summary>
    /// Disable the attack collider attached with this gameObject.
    /// </summary>
    public void DisableAttackCollider()
    {
        attackCollider.enabled = false;
    }

    private void ShowHitVFX(ContactPoint contactPoint)
    {
        if (!hitVFX)
            return;

        Vector3 position = contactPoint.point;
        Quaternion rotation = Quaternion.FromToRotation(position, position + contactPoint.normal);
        Instantiate(hitVFX, position, rotation);
    }
    
    private void ShowHitVFX(Vector3 pos)
    {
        if (!hitVFX)
            return;

        ParticleSystem spawnedVFX = Instantiate(hitVFX, pos, quaternion.identity);
        ParticleSystem.MainModule vfxMainModule = spawnedVFX.main;
        Destroy(spawnedVFX.gameObject, vfxMainModule.duration);
    }
    

}
