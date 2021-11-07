using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoverState : StateMachineBehaviour
{
    private int hasBeenResetAnimID;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        hasBeenResetAnimID = Animator.StringToHash("hasBeenReset");
        animator.SetBool(hasBeenResetAnimID,false);
    }
    
   
}
