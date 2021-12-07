using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ArtificialGravityState : StateMachineBehaviour
{
    [SerializeField] private bool applyArtificialGravity;
    private CharacterController characterController;
    private bool stateHasExited = false;
    
    public new virtual void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        characterController = animator.GetComponent<CharacterController>();
        stateHasExited = false;
    }

    public new virtual void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateHasExited)
            return;
        
        base.OnStateMove(animator, stateInfo, layerIndex);
        if (applyArtificialGravity)
        {
            //Don't know why characterController will be null very rarely 
            if(!characterController )
                characterController = animator.GetComponent<CharacterController>();
            characterController.Move(new Vector3(0, -15f, 0f) * Time.deltaTime);
        }
    }

    public new virtual void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        stateHasExited = true;
    }
}
