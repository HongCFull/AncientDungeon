using System.Collections;
using System.Collections.Generic;
using TPSTemplate;
using UnityEngine;

public class EnterLocoMotionState : StateMachineBehaviour
{
    private ThirdPersonController tpsController = null;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        //Debug.Log("On loco motion state");
        DisableRootMotion(animator);
        EnableTPSMovement(animator);
    }

    private void DisableRootMotion(Animator animator)
    {
        animator.applyRootMotion = false;
    }
    
    private void EnableTPSMovement(Animator animator)
    {
        if (!tpsController)
            tpsController = animator.GetComponent<ThirdPersonController>();
        tpsController.EnableCharacterMovement();
    }
}
