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
    private PlayerCharacter playerCharacter;
    
    private bool originalRMOption;
    private bool hasExited;

    private float dashVelocity;
    private Vector3 dashDirection;
    
    private int animIDStateCanBeInterrupted;

        
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        InitializeVariables(animator,stateInfo);

        animator.applyRootMotion = true;
        animator.SetBool(animIDStateCanBeInterrupted,false);
        
        tpsController.ForceDisableCharacterWalking();
        tpsController.DisableGravity();
        
        playerCharacter.canBeDamaged = false;

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
        
        animator.SetBool(animIDStateCanBeInterrupted,true);
        animator.applyRootMotion = originalRMOption;
        
        tpsController.ForceEnableCharacterWalking();
        tpsController.EnableGravity();
        playerCharacter.canBeDamaged = true;

    }

    void InitializeVariables(Animator animator,AnimatorStateInfo stateInfo)
    {
        tpsController = animator.GetComponent<ThirdPersonController>();
        characterController = animator.GetComponent<CharacterController>();
        playerCharacter = animator.GetComponent<PlayerCharacter>();
        
        hasExited = false;
        originalRMOption = animator.applyRootMotion;
        dashVelocity = dashDistance / stateInfo.length;
        
        dashDirection = animator.transform.forward;
        dashDirection.y = 0;
        dashDirection = dashDirection.normalized;
        
        animIDStateCanBeInterrupted = Animator.StringToHash("StateCanBeInterrupted");

    }
}
