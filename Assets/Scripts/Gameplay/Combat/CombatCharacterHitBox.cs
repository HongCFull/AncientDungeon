using System;
using System.Collections;
using System.Collections.Generic;
using Combat;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 
/// </summary>

[RequireComponent(typeof(Collider))]
public class CombatCharacterHitBox : HitBox
{
    [Header("CombatCharacterHitBox Settings")] 
    [SerializeField] private CombatCharacter parentCombatCharacter;
    

    public override void TakeDamageBy(float damage)
    {
        parentCombatCharacter.TakeDamageBy(damage);
    }
    
}
