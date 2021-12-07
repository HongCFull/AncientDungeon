using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShirleySprialSlash : ShirleySkill
{
    [SerializeField] protected PlayerCharacter playerCharacter;
    [SerializeField] private ShirleySkillUISlot skillUISlot;

    public override void PerformSkill()
    {
        if ( !canPerformSkill)
            return;

        if (playerCharacter.AnimatorStateCanBeInterrupted() &&
            playerCharacter.AnimatorIsGrounded()) {
            
            //Debug.Log("Called spiral");
            
            canPerformSkill = false;
            skillUISlot.StartCoolDownFilterWrapper(coolDownInSec);
            playerCharacter.SetAnimatorStateTo(hashedAnimStateID);
        }
            
        
    }
}
