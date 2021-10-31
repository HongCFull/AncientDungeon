using System.Collections;
using System.Collections.Generic;
using TPSTemplate;
using UnityEngine;

public class DashInAirState : StateMachineBehaviour
{

    [SerializeField] private float dashDistance;
    
    private bool originalRMOption;
    private int animIDCachedDashAngle;
    private int animIDTurningAngle;
    private ThirdPersonController tpsController;

    private bool hasExited; 
        
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        InitializeVariables(animator);
        
        animator.SetFloat(animIDCachedDashAngle,animator.GetFloat(animIDTurningAngle));
        animator.applyRootMotion = true;   
        
        tpsController.DisableGravity();
    }

    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateMove(animator, stateInfo, layerIndex);
    }
    
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        animator.applyRootMotion = originalRMOption;
        animator.SetFloat(animIDCachedDashAngle,0f);
        tpsController.EnableGravity();
    }

    void InitializeVariables(Animator animator)
    {
        tpsController = animator.GetComponent<ThirdPersonController>();

        animIDCachedDashAngle = Animator.StringToHash("CachedDashAngle");
        animIDTurningAngle = Animator.StringToHash("TurningAngle");

        hasExited = false;
        originalRMOption = animator.applyRootMotion;

    }
}
