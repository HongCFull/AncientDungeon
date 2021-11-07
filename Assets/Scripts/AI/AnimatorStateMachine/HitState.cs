using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class HitState : StateMachineBehaviour
{
    
    private int isDamagedAnimID;
    private ThirdPersonController tpsController;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        InitializeVariables(animator);
        //animator.ResetTrigger(isDamagedAnimID);
        
        tpsController.DisableCharacterWalking();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        tpsController.EnableCharacterWalking();

    }

    void InitializeVariables(Animator animator)
    {
        isDamagedAnimID = Animator.StringToHash("isDamagedAnimID");
        tpsController = animator.gameObject.GetComponent<ThirdPersonController>();
    }
}
