using UnityEngine;

namespace HitBoxDefinition
{
    [RequireComponent(typeof(Collider))]
    public class ReceiveHitBox : HitBox
    {
        [Header("CombatCharacterHitBox Settings")] 
        [SerializeField] private CombatCharacter parentCombatCharacter;

        public CombatCharacter GetCombatCharacterOwner() => parentCombatCharacter;
        
        public void EnableHitBox(bool enable)=> this.enabled = enable;

    }
    
}
