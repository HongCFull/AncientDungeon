using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Player;
using UnityEngine;

public class DashInAirState : StateMachineBehaviour
{

    [SerializeField] private float dashDistance;
    
    private ThirdPersonController tpsController;
    private CharacterController characterController;
    
    private bool originalRMOption;
    private bool hasExited;

    private float dashVelocity;
    private Vector3 dashDirection;
    
        
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        InitializeVariables(animator,stateInfo);

        tpsController.ForceDisableCharacterWalking();
        tpsController.DisableGravity();
        animator.applyRootMotion = true;

    }

    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateMove(animator, stateInfo, layerIndex);

        if (hasExited)
            return;

        characterController.Move(Time.deltaTime * dashVelocity * dashDirection);
        //animator.transform.position += Time.deltaTime * dashVelocity * dashDirection;

    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        hasExited = true;
        
        animator.applyRootMotion = originalRMOption;
        
        tpsController.ForceEnableCharacterWalking();
        tpsController.EnableGravity();
    }

    void InitializeVariables(Animator animator,AnimatorStateInfo stateInfo)
    {
        tpsController = animator.GetComponent<ThirdPersonController>();
        characterController = animator.GetComponent<CharacterController>();
        
        hasExited = false;
        originalRMOption = animator.applyRootMotion;
        dashVelocity = dashDistance / stateInfo.length;
        
        dashDirection = animator.transform.forward;
        dashDirection.y = 0;
        dashDirection = dashDirection.normalized;
    }
}
