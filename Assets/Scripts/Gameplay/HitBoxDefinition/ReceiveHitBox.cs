using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HitBoxDefinition
{
    [RequireComponent(typeof(Collider))]
    public class ReceiveHitBox : HitBox
    {
        [Header("CombatCharacterHitBox Settings")] 
        [SerializeField] private CombatCharacter parentCombatCharacter;

        public CombatCharacter GetCombatCharacterOwner() => parentCombatCharacter;

        private void Awake()
        {
            parentCombatCharacter.RegisterAsHitBox(this);
        }

        public void EnableHitBox(bool enable)
        {
            this.enabled = enable;
        }
        
        /*
        public override void TakeDamageBy(float damage)
        {
            parentCombatCharacter.TakeDamageBy(damage);
        }
        */
        
    }
    
}
