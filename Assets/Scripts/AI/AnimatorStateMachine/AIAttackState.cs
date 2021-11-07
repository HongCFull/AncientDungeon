using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttackState : StateMachineBehaviour
{

    [SerializeField] private bool canBeInterrupted;
    private bool originalIsInvulnerableState;
    private int animID_isInvulnerable;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        animID_isInvulnerable = Animator.StringToHash("isInvulnerable");
        originalIsInvulnerableState = animator.GetBool(animID_isInvulnerable);
        animator.SetBool(animID_isInvulnerable,!canBeInterrupted);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        animator.SetBool(animID_isInvulnerable,originalIsInvulnerableState);
    }
}
