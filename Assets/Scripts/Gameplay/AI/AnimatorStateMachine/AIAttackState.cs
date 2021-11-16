using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttackState : StateMachineBehaviour
{

    [SerializeField] private bool canBeInterrupted;
    private bool orginalInterruptableState;
    private int animIDStateCanBeInterrupted;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        animIDStateCanBeInterrupted = Animator.StringToHash("stateCanBeInterrupted");
        orginalInterruptableState = animator.GetBool(animIDStateCanBeInterrupted);
        animator.SetBool(animIDStateCanBeInterrupted,canBeInterrupted);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        animator.SetBool(animIDStateCanBeInterrupted,orginalInterruptableState);
    }
}
