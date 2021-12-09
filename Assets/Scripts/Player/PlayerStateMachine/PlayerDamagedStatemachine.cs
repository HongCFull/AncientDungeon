using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using UnityEngine.Animations;


public class PlayerDamagedStatemachine : StateMachineBehaviour
{
    private ThirdPersonController tpsController;
    private int animID_Dash;
    private int animID_MeleeAttack;
    private int animID_CanAttack;
    
    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        base.OnStateMachineEnter(animator, stateMachinePathHash);

        InitializeVariables(animator);
        ResetInputTriggers(animator);
        tpsController.ForceDisableCharacterWalking();

    }

    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash, AnimatorControllerPlayable controller)
    {
        base.OnStateMachineExit(animator, stateMachinePathHash, controller);
        tpsController.ForceEnableCharacterWalking();

    }

    void InitializeVariables(Animator animator)
    {
        tpsController = animator.gameObject.GetComponent<ThirdPersonController>();
        animID_Dash = Animator.StringToHash("dash");
        animID_MeleeAttack = Animator.StringToHash("meleeAttack");
        animID_CanAttack = Animator.StringToHash("canAttack");
    }

    void ResetInputTriggers(Animator animator)
    {
        animator.ResetTrigger(animID_Dash);
        animator.ResetTrigger(animID_CanAttack);
        animator.ResetTrigger(animID_MeleeAttack);
    }
}
