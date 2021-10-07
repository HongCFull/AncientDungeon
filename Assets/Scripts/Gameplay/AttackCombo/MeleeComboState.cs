using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeComboState : StateMachineBehaviour
{
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        ResetMeleeAttack(animator);
    }

    void ResetMeleeAttack(Animator animator) {
        animator.ResetTrigger("MeleeAttack");
        Debug.Log("Reset MeleeAttack bool");   
    }
}
