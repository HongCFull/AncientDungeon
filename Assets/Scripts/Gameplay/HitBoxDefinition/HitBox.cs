using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HitBoxDefinition
{
    [RequireComponent(typeof(Collider))]
    public abstract class HitBox : MonoBehaviour
    {
        [Header("Generic HitBox Settings")]
        [Tooltip("The Tag of this HitBox")]
        [SerializeField] private HitBoxTag hitBoxTag;
        [SerializeField] protected CombatCharacter owner;
        [SerializeField] protected Collider hitBoxCollider;
        
        public HitBoxTag GetHitBoxTag() => hitBoxTag;
        public CombatCharacter GetCombatCharacterOwner() => owner;

        protected virtual void Awake()
        {
            if(!owner)
                throw new Exception("CombatCharacter owner of "+this.gameObject.name+" is not assigned");

            if (!hitBoxCollider) {
                if(owner)
                    throw new Exception(owner.gameObject.name+"'s hitCollider is not assigned");
                else
                    throw new Exception("HitBoxCollider owner is not assigned");
            }
        }
    }
    
}
