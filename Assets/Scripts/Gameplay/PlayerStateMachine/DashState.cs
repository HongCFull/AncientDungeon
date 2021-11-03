using System.Collections;
using System.Collections.Generic;
using System.Timers;
using TPSTemplate;
using UnityEngine;

public class DashState : StateMachineBehaviour
{
    private bool originalRMOption;
    private int animIDCachedDashAngle;
    private int animIDTurningAngle;
    private ThirdPersonController tpsController;
    private CharacterController characterController;

     public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        InitializeVariables(animator);
 
        animator.SetFloat(animIDCachedDashAngle,animator.GetFloat(animIDTurningAngle));
        animator.applyRootMotion = true;
        tpsController.DisableCharacterWalking();

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
        
        tpsController.EnableCharacterWalking();
        animator.applyRootMotion = originalRMOption;
        animator.SetFloat(animIDCachedDashAngle,0f); 

     }

     private void InitializeVariables(Animator animator)
     {
         tpsController = animator.GetComponent<ThirdPersonController>();
         characterController = animator.GetComponent<CharacterController>();
         
         animIDCachedDashAngle = Animator.StringToHash("CachedDashAngle");
         animIDTurningAngle = Animator.StringToHash("TurningAngle");
         
         originalRMOption = animator.applyRootMotion;

     }
}
