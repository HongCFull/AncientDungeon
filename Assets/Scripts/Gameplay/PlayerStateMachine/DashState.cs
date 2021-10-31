using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashState : StateMachineBehaviour
{
    private bool originalRMOption;
    private int animIDCachedDashAngle;
    private int animIDTurningAngle;

     public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        animIDCachedDashAngle = Animator.StringToHash("CachedDashAngle");
        animIDTurningAngle = Animator.StringToHash("TurningAngle");
        
        animator.SetFloat(animIDCachedDashAngle,animator.GetFloat(animIDTurningAngle));
        originalRMOption = animator.applyRootMotion;
        animator.applyRootMotion = true;
Debug.Log("enter dash state with rm: "+animator.applyRootMotion);
        
    }
     

     public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        Debug.Log("exit dash state with rm"+animator.applyRootMotion);
        animator.applyRootMotion = originalRMOption;
        animator.SetFloat(animIDCachedDashAngle,0f); 

    }
}
