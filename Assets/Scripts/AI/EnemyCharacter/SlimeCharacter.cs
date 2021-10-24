using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeCharacter : AICharacter
{

    [Header("Attack Settings")] 
    [SerializeField] private List<Collider> attackColliders;
    [Header("Flee Setting")]
    [SerializeField] private float fleeDistance;
    
    public float GetFleeDistance() => fleeDistance;


    protected override void DisableAllAttackColliders()
    {
        foreach (Collider collider in attackColliders) {
            collider.enabled = false;
        }
    }
    
    protected override void EnableAttackCollider(int i)
    {
        if (i < 0 || i >= attackColliders.Count)
            return;
        
        attackColliders[i].enabled = true;
    }
    
    protected override void DisableAttackCollider(int i)
    {
        if (i < 0 || i >= attackColliders.Count)
            return;
        
        attackColliders[i].enabled = false;
    }
}
