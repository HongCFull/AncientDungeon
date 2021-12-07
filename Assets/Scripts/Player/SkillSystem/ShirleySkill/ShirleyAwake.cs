using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

/// <summary>
/// A Special case of shirley skills which is not replaceable
/// </summary>
public class ShirleyAwake : ShirleySkill
{
    [SerializeField] [Range(0,Mathf.Infinity)] private float awakeModeDuration;
    [SerializeField] private ShirleyAwakeUISlot awakeUISlot;
    [SerializeField] protected PlayerCharacter playerCharacter;

    public override void PerformSkill()
    {
        if (!canPerformSkill)
            return;

        if (playerCharacter.AnimatorStateCanBeInterrupted() &&
            playerCharacter.AnimatorIsGrounded() &&
            playerCharacter.playerAttackMode == PlayerAttackMode.NORMAL) {
            
            playerCharacter.SetCharacterToAwakeMode();
            awakeUISlot.StartAwakeCoolDownFilterWrapper(coolDownInSec);
            canPerformSkill = false;
            
            StartCoroutine(ExitAwakeMode(awakeModeDuration));
        }
            
    }

    IEnumerator ExitAwakeMode(float delay)
    {
        yield return new WaitForSeconds(delay);
        playerCharacter.SetCharacterToNormalMode();
    }
}
