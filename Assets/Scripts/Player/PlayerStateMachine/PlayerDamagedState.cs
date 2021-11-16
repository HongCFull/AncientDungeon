using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class PlayerDamagedState : ArtificialGravityState
{
    private bool hasExited;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        InitializeVariables(animator);
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
    }

    void InitializeVariables(Animator animator)
    {
        hasExited = false;
    }
}
