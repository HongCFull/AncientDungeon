using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacterSlime : AICharacter
{
    
    [Header("Flee Setting")]
    [SerializeField] private float fleeDistance;
    
    public float GetFleeDistance() => fleeDistance;

    public override void DisableAllAttackHitBoxes()
    {
        foreach (AttackHitBox atkHitBox in attackHitBoxes)
            atkHitBox.DisableAttackCollider();
        
    }
}
