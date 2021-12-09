using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class PlayerDamagedState : ArtificialGravityState
{
    
    //[SerializeField] private bool stateCanBeInterrupted =true;
    private ThirdPersonController tpsController;
    private int animID_StateCanBeInterrupted;
    private bool originalOptionStateCanBeInterrupted;
    private bool hasExited;
    
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        InitializeVariables(animator);
        tpsController.ForceDisableCharacterWalking();
        animator.SetBool(animID_StateCanBeInterrupted,false);
    }
    
    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (hasExited)
            return;
        
        base.OnStateMove(animator, stateInfo, layerIndex);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        hasExited = true;
        animator.SetBool(animID_StateCanBeInterrupted,originalOptionStateCanBeInterrupted);
        //tpsController.ForceEnableCharacterWalking();
    }

    void InitializeVariables(Animator animator)
    {
        if(!tpsController)
            tpsController = animator.GetComponent<ThirdPersonController>();
        
        hasExited = false;
        animID_StateCanBeInterrupted = Animator.StringToHash("stateCanBeInterrupted");
        originalOptionStateCanBeInterrupted = animator.GetBool(animID_StateCanBeInterrupted);
    }
}
