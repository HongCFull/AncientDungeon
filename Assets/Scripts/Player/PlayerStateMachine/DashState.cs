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
    private ThirdPersonController tpsController;
    private CharacterController characterController;
    private PlayerCharacter playerCharacter;

     public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        InitializeVariables(animator);
 
        animator.SetFloat(animIDCachedDashAngle,animator.GetFloat(animIDTurningAngle));
        animator.applyRootMotion = true;
        tpsController.ForceDisableCharacterWalking();
        playerCharacter.canBeDamaged = false;
        //Debug.Log("enter dash state with rm: "+animator.applyRootMotion);

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
        playerCharacter.canBeDamaged = true;
        animator.applyRootMotion = originalRMOption;
        animator.SetFloat(animIDCachedDashAngle,0f); 

     }

     private void InitializeVariables(Animator animator)
     {
         tpsController = animator.GetComponent<ThirdPersonController>();
         characterController = animator.GetComponent<CharacterController>();
         playerCharacter = animator.GetComponent<PlayerCharacter>();
         
         animIDCachedDashAngle = Animator.StringToHash("CachedDashAngle");
         animIDTurningAngle = Animator.StringToHash("TurningAngle");
         
         originalRMOption = animator.applyRootMotion;

     }
}
