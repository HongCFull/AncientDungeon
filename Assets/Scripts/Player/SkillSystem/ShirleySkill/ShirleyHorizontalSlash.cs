using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShirleyHorizontalSlash : ShirleySkill
{
    [SerializeField] protected PlayerCharacter playerCharacter;
    [SerializeField] private ShirleySkillUISlot skillUISlot;

    public override void PerformSkill()
    {
        if (!canPerformSkill || playerCharacter.IsDead())
            return;
        
        if (playerCharacter.AnimatorStateCanBeInterrupted() &&
            playerCharacter.AnimatorIsGrounded()) {
            canPerformSkill = false;
            skillUISlot.StartCoolDownFilterWrapper(coolDownInSec);
            playerCharacter.SetAnimatorStateTo(hashedAnimStateID);
        }
    }
}
