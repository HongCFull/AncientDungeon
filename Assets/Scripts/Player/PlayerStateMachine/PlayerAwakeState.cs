using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerAwakeState : StateMachineBehaviour
{
    private ThirdPersonController tpsController;
    private PlayerCharacter playerCharacter;
    private int animID_StateCanBeInterrupted;

    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        Initialization(animator);
        
        animator.SetBool(animID_StateCanBeInterrupted,false);
        tpsController.ForceDisableCharacterWalking();
        playerCharacter.SetCombatCharacterToInvulnerable(true);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        
        animator.SetBool(animID_StateCanBeInterrupted,true);
        tpsController.ForceEnableCharacterWalking();
        
        playerCharacter.SetCombatCharacterToInvulnerable(false);
    }

    void Initialization(Animator animator)
    {
        playerCharacter = animator.GetComponent<PlayerCharacter>();
        tpsController = animator.GetComponent<ThirdPersonController>();
        animID_StateCanBeInterrupted = Animator.StringToHash("stateCanBeInterrupted");
    }
}
