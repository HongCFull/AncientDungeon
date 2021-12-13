using System;
using System.Collections;
using System.Collections.Generic;
using HitBoxDefinition;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
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

    public override void DisableAllReceiveHitBoxes()
    {
        foreach (ReceiveHitBox receiveHitBox in receiveHitBoxes)
            receiveHitBox.DisableReceiveHitBox();        
        
    }
    

}
