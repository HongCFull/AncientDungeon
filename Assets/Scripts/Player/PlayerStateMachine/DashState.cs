using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Player;
using UnityEngine;

public class DashState : StateMachineBehaviour
{
    private bool originalRMOption;
    private int animIDCachedDashAngle;
    private int animIDTurningAngle;
    private int animIDStateCanBeInterrupted;
    
    private ThirdPersonController tpsController;
    private CharacterController characterController;
    private PlayerCharacter playerCharacter;

     public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        InitializeVariables(animator);
 
        animator.SetBool(animIDStateCanBeInterrupted,false);
        animator.SetFloat(animIDCachedDashAngle,animator.GetFloat(animIDTurningAngle));
        animator.applyRootMotion = true;
        
        tpsController.ForceDisableCharacterWalking();
        playerCharacter.SetCombatCharacterToInvulnerable(true);
        
    }

     public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
     {
         base.OnStateMove(animator, stateInfo, layerIndex);
         animator.ApplyBuiltinRootMotion();
         characterController.Move(new Vector3(0, -10f, 0) * Time.deltaTime);

     }

     public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
     {
        base.OnStateExit(animator, stateInfo, layerIndex);
       // Debug.Log("exit dash state with rm"+animator.applyRootMotion);
        
        tpsController.ForceEnableCharacterWalking();
        playerCharacter.SetCombatCharacterToInvulnerable(false);
        
        animator.applyRootMotion = originalRMOption;
        animator.SetFloat(animIDCachedDashAngle,0f);
        animator.SetBool(animIDStateCanBeInterrupted,true);

     }

     private void InitializeVariables(Animator animator)
     {
         tpsController = animator.GetComponent<ThirdPersonController>();
         characterController = animator.GetComponent<CharacterController>();
         playerCharacter = animator.GetComponent<PlayerCharacter>();
         
         animIDCachedDashAngle = Animator.StringToHash("cachedDashAngle");
         animIDTurningAngle = Animator.StringToHash("turningAngle");
         animIDStateCanBeInterrupted = Animator.StringToHash("stateCanBeInterrupted");
         
         originalRMOption = animator.applyRootMotion;

     }
}
