using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class PlayerDeathState : StateMachineBehaviour
{
    private ThirdPersonController tpsController;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        tpsController = animator.GetComponent<ThirdPersonController>();
        
        tpsController.ForceDisableCharacterWalking();
    }
    
    
}
