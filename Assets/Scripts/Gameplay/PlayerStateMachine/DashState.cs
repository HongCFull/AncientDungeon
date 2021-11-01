using System.Collections;
using System.Collections.Generic;
using TPSTemplate;
using UnityEngine;

public class DashState : StateMachineBehaviour
{
    private bool originalRMOption;
    private int animIDCachedDashAngle;
    private int animIDTurningAngle;
    private ThirdPersonController tpsController;

     public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        InitializeVariables(animator);
 
        animator.SetFloat(animIDCachedDashAngle,animator.GetFloat(animIDTurningAngle));
        animator.applyRootMotion = true;
        tpsController.DisableCharacterWalking();

        //Debug.Log("enter dash state with rm: "+animator.applyRootMotion);
        
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
         
         animIDCachedDashAngle = Animator.StringToHash("CachedDashAngle");
         animIDTurningAngle = Animator.StringToHash("TurningAngle");
         
         originalRMOption = animator.applyRootMotion;

     }
}
