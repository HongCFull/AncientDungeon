using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class PlayerDamagedStatemachine : StateMachineBehaviour
{
    private ThirdPersonController tpsController;
    

    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        base.OnStateMachineEnter(animator, stateMachinePathHash);
        tpsController = animator.gameObject.GetComponent<ThirdPersonController>();
        tpsController.DisableCharacterWalking();
    }
    
}
