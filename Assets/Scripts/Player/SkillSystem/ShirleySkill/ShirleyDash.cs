using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class ShirleyDash : ShirleySkill
{
    [SerializeField] protected PlayerCharacter playerCharacter;
    [SerializeField] protected ThirdPersonController tpsController;
    [SerializeField] private ShirleySkillUISlot skillUISlot;

    public override void PerformSkill()
    {
        if (playerCharacter.IsDead() || !canPerformSkill)
            return;

        if (playerCharacter.AnimatorStateCanBeInterrupted()) {
            skillUISlot.StartCoolDownFilterWrapper(coolDownInSec);
            canPerformSkill = false;
            tpsController.TriggerDash();
        }
    }

}
